﻿using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsolePlatformer
{
    class Menu
    {
        private Game game;
        private Player player;
        private IEnumerable<IWeapon> filteredInventory;
        private Dictionary<int, IWeapon> weaponDict;
        private int LeftBound;
        private int RightBound;
        private int Top;
        private int Bottom;
        private bool menuOpen;
        private string header = "";

        public Menu(Player player, Game game)
        {
            this.game = game;
            this.player = player;
            filteredInventory = player.Inventory;
            weaponDict = new Dictionary<int, IWeapon>();
            Top = 3;
            LeftBound = 3;
            Bottom = 28;
            RightBound = 111;
            menuOpen = true;
            OpenMenu();
            if (player.CurrentHealth > 0)
                PrintOptions();
            else
                DrawGameResults();
        }

        public void OpenMenu()
        {
            Console.SetCursorPosition(LeftBound, Top);
            Console.BackgroundColor = ConsoleColor.White;
            for(int i = LeftBound; i <= RightBound; i++)
            {
                Console.Write(' ');
            }
            for(int i = Top + 1; i < Bottom; i++)
            {
                Console.SetCursorPosition(LeftBound, i);
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.Black;
                for(int j = LeftBound + 1; j < RightBound; j++) 
                    Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(' ');
            }
            Console.SetCursorPosition(LeftBound, Bottom);
            for (int i = LeftBound; i <= RightBound; i++)
            {
                Console.Write(' ');
            }
        }

        private void PrintOptions()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(GetCenter("Inventory"), Top + 3);
            Console.Write("Inventory");
            Console.SetCursorPosition(GetCenter("-----------"), Top + 4);
            Console.Write("-----------");
            Console.SetCursorPosition(LeftBound + 1, Top + 6);
            Console.Write($"{' ', -20}M - Machine Guns{' ', -25}C - Common Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 9);
            Console.Write($"{' ', -20}S - Shotguns{' ', -29}R - Rare Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 12);
            Console.Write($"{' ', -20}B - Rocket Launchers {' ',-20}L - Legendary Weapons");
            Console.SetCursorPosition(GetCenter("O - Purchase Random Weapon for $1000"), Top + 15);
            Console.Write("O - Purchase Random Weapon for $1000");
            Console.SetCursorPosition(GetCenter("H - Purchase +10 Max Health for $1000"), Top + 18);
            Console.Write("H - Purchase +10 Max Health for $1000");
            Console.SetCursorPosition(GetCenter("Enter to Close Menu"), Bottom - 3);
            Console.Write("Enter to Close Menu");

            NavigateMenu();
        }

        private void DrawGameResults()
        {

        }

        private void NavigateMenu()
        {
            while (menuOpen)
            {
                Console.SetCursorPosition(LeftBound + 1, Top + 9);
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.M:
                            GetMachineGuns();
                            break;
                        case ConsoleKey.C:
                            GetCommons();
                            break;
                        case ConsoleKey.S:
                            GetShotguns();
                            break;
                        case ConsoleKey.R:
                            GetRares();
                            break;
                        case ConsoleKey.B:
                            GetLaunchers();
                            break;
                        case ConsoleKey.L:
                            GetLegendaries();
                            break;
                        case ConsoleKey.Enter:
                            menuOpen = false;
                            break;
                    }
                }
            }
            ClearMenu();
            game.Go();
        }

        private void ClearMenu()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = Top; i <= Bottom; i++)
            {
                Console.SetCursorPosition(LeftBound, i);
                for (int j = LeftBound; j <= RightBound; j++)
                {
                    Console.Write(' ');
                }
            }
        }

        private void GetLegendaries()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Rarity == Rarities.LEGENDARY
                select w;

            header = header.Length > 0 ? $"Legendary {header}" : "Legendary";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void UpdateMenuSelections(IEnumerable<IWeapon> filteredWeapons)
        {
            string divider = "--------------------------\n";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(GetCenter(header), Top + 3);
            Console.Write(header);
            Console.SetCursorPosition(GetCenter(divider), Top + 4);
            Console.Write(divider);
            int currentLine = Top + 6;
            int lineNum = 1;
            foreach (IWeapon weapon in filteredWeapons)
            {
                Console.SetCursorPosition(33, currentLine);
                Console.Write($"{lineNum}: ");
                Console.WriteLine(weapon);
                weaponDict[lineNum] = weapon;
                currentLine++;
                lineNum++;
            }
            string controls = "Up/Down to Select -- SpaceBar to Equip -- Enter to Go Back";
            Console.SetCursorPosition(GetCenter(controls), Bottom - 3);
            Console.Write(controls);
        }

        private int GetCenter(string myString)
        {
            return (RightBound - LeftBound) / 2 - myString.Length / 2;
        }

        private void GetLaunchers()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Type == WeaponTypes.ROCKETLAUNCHER
                select w;

            header = header.Length > 0 ? $"{header} Rocket Launchers" : "Rocket Launchers";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetRares()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Rarity == Rarities.RARE
                select w;

            header = header.Length > 0 ? $"Rare {header}" : "Rare";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetShotguns()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Type == WeaponTypes.SHOTGUN
                select w;

            header = header.Length > 0 ? $"{header} Shotguns" : "Shotguns";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetCommons()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Rarity == Rarities.COMMON
                select w;

            header = header.Length > 0 ? $"Common {header}" : "Common";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetMachineGuns()
        {
            filteredInventory =
                from w in filteredInventory
                where w.Type == WeaponTypes.MACHINEGUN
                select w;

            header = header.Length > 0 ? $"{header} Machine Guns" : "Machine Guns";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }
    }
}
