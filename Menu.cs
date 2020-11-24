using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsolePlatformer
{
    class Menu
    {
        private Game game;
        private Player player;
        private List<IWeapon> filteredInventory;
        private Dictionary<int, IWeapon> weaponDict;
        private int LeftBound;
        private int RightBound;
        private int Top;
        private int Bottom;
        private bool menuOpen;
        private string header;
        private int itemNumber;
        private bool onInventoryScreen;

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
            header = "";
            itemNumber = 0;
            onInventoryScreen = false;
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

        public void PrintOptions()
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

        public void DrawGameResults()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(GetCenter("YOU DIED!"), Top + 3);
            Console.Write("YOU DIED!");
            Console.SetCursorPosition(GetCenter("-----------"), Top + 4);
            Console.Write("-----------");
            Console.SetCursorPosition(GetCenter($"You Have {player.Cash} Cash"), Top + 6);
            Console.Write($"You Have {player.Cash} Cash");
            Console.SetCursorPosition(GetCenter($"You Reached Level {Game.Level}"), Top + 9);
            Console.Write($"You Reached Level {Game.Level}");
            Console.SetCursorPosition(GetCenter("Press Any Button To Exit"), Bottom - 3);
            Console.Write("Press Any Button To Exit");
        }

        private void NavigateMenu()
        {
            while (menuOpen)
            {
                int currentLine = Top + 6;
                Console.SetCursorPosition(LeftBound + 1, Top + 9);
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.M:
                            GetMachineGuns();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.C:
                            GetCommons();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.S:
                            GetShotguns();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.R:
                            GetRares();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.B:
                            GetLaunchers();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.L:
                            GetLegendaries();
                            onInventoryScreen = true;
                            break;
                        case ConsoleKey.H:
                            if (!onInventoryScreen)
                                player.UpgradeHealth();
                            break;
                        case ConsoleKey.UpArrow:
                            if (onInventoryScreen)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                if (itemNumber > 0 && filteredInventory.Count() > 0)
                                {
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.Write(filteredInventory[itemNumber]);
                                    itemNumber--;
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(filteredInventory[itemNumber]);
                                }
                                else if (filteredInventory.Count() > 0)
                                {
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.Write(filteredInventory[itemNumber]);
                                    itemNumber = filteredInventory.Count() - 1;
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(filteredInventory[itemNumber]);
                                }
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (onInventoryScreen)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;
                                if (itemNumber < filteredInventory.Count() - 1 && filteredInventory.Count() > 0)
                                {
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.Write(filteredInventory[itemNumber]);
                                    itemNumber++;
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(filteredInventory[itemNumber]);
                                }
                                else if (filteredInventory.Count() > 0)
                                {
                                    Console.SetCursorPosition(33, currentLine + itemNumber);
                                    Console.Write(filteredInventory[itemNumber]);
                                    itemNumber = 0;
                                    Console.SetCursorPosition(33, currentLine);
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.Write(filteredInventory[itemNumber]);
                                }
                            }
                            break;
                        case ConsoleKey.Spacebar:
                            if (onInventoryScreen)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                filteredInventory[itemNumber].Equip();
                                player.EquipWeapon(filteredInventory[itemNumber]);
                            }
                            break;
                        case ConsoleKey.Enter:
                            menuOpen = false;
                            onInventoryScreen = false;
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
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Rarity == Rarities.LEGENDARY
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
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
                Console.WriteLine(weapon);
                weaponDict[lineNum] = weapon;
                currentLine++;
                lineNum++;
            }
            string controls = "Up/Down to Select -- SpaceBar to Equip -- Enter to Exit";
            Console.SetCursorPosition(GetCenter(controls), Bottom - 3);
            Console.Write(controls);
        }

        private int GetCenter(string myString)
        {
            return (RightBound - LeftBound) / 2 - myString.Length / 2;
        }

        private void GetLaunchers()
        {
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Type == WeaponTypes.ROCKETLAUNCHER
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
            header = header.Length > 0 ? $"{header} Rocket Launchers" : "Rocket Launchers";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetRares()
        {
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Rarity == Rarities.RARE
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
            header = header.Length > 0 ? $"Rare {header}" : "Rare";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetShotguns()
        {
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Type == WeaponTypes.SHOTGUN
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
            header = header.Length > 0 ? $"{header} Shotguns" : "Shotguns";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetCommons()
        {
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Rarity == Rarities.COMMON
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
            header = header.Length > 0 ? $"Common {header}" : "Common";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }

        private void GetMachineGuns()
        {
            IEnumerable<IWeapon> filtered =
                from w in filteredInventory
                where w.Type == WeaponTypes.MACHINEGUN
                select w;

            filteredInventory = filtered.ToList<IWeapon>();
            header = header.Length > 0 ? $"{header} Machine Guns" : "Machine Guns";
            OpenMenu();
            UpdateMenuSelections(filteredInventory);
        }
    }
}
