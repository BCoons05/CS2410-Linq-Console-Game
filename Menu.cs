using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

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
        private bool onMainMenu;
        private IWeapon newWeapon;
        private Random rnd;

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
            onMainMenu = true;
            rnd = new Random();
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
            Console.SetCursorPosition(GetCenter("Inventory", LeftBound, RightBound), Top + 3);
            Console.Write("Inventory");
            Console.SetCursorPosition(GetCenter("-----------", LeftBound, RightBound), Top + 4);
            Console.Write("-----------");
            Console.SetCursorPosition(LeftBound + 1, Top + 6);
            Console.Write($"{' ', -20}M - Machine Guns{' ', -25}C - Common Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 9);
            Console.Write($"{' ', -20}S - Shotguns{' ', -29}R - Rare Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 12);
            Console.Write($"{' ', -20}B - Rocket Launchers {' ',-20}L - Legendary Weapons");
            Console.SetCursorPosition(GetCenter("O - Purchase Random Weapon for $1000", LeftBound, RightBound), Top + 15);
            Console.Write("W - Purchase Random Weapon for $1000");
            Console.SetCursorPosition(GetCenter("H - Purchase +10 Max Health for $1000", LeftBound, RightBound), Top + 18);
            Console.Write("H - Purchase +10 Max Health for $1000");
            Console.SetCursorPosition(GetCenter("Enter to Close Menu", LeftBound, RightBound), Bottom - 3);
            Console.Write("Enter to Close Menu");

            onMainMenu = true;
            NavigateMenu();
        }

        public void DrawGameResults()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(GetCenter("YOU DIED!", LeftBound, RightBound), Top + 3);
            Console.Write("YOU DIED!");
            Console.SetCursorPosition(GetCenter("-----------", LeftBound, RightBound), Top + 4);
            Console.Write("-----------");
            Console.SetCursorPosition(GetCenter($"You Have {player.Cash} Cash", LeftBound, RightBound), Top + 6);
            Console.Write($"You Have {player.Cash} Cash");
            Console.SetCursorPosition(GetCenter($"You Reached Level {Game.Level}", LeftBound, RightBound), Top + 9);
            Console.Write($"You Reached Level {Game.Level}");
            Console.SetCursorPosition(GetCenter("Press Any Button To Exit", LeftBound, RightBound), Bottom - 3);
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
                            onMainMenu = false;
                            break;
                        case ConsoleKey.C:
                            GetCommons();
                            onMainMenu = false;
                            break;
                        case ConsoleKey.S:
                            GetShotguns();
                            onMainMenu = false;
                            break;
                        case ConsoleKey.R:
                            GetRares();
                            onMainMenu = false;
                            break;
                        case ConsoleKey.B:
                            GetLaunchers();
                            onMainMenu = false;
                            break;
                        case ConsoleKey.L:
                            GetLegendaries();
                            onMainMenu = false;
                            break;
                        case ConsoleKey.H:
                            if (onMainMenu && player.Cash >= 1000)
                            {
                                player.UpgradeHealth();
                                PurchaseConfirmation("Health increased by 10!");
                            }  
                            break;
                        case ConsoleKey.W:
                            if (onMainMenu && player.Cash >= 1000)
                            {
                                newWeapon = CreateNewWeapon();
                                player.Inventory.Add(newWeapon);
                                PurchaseConfirmation(newWeapon.ToString());
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (!onMainMenu)
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
                            if (!onMainMenu)
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
                            if (!onMainMenu)
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                filteredInventory[itemNumber].Equip();
                                player.EquipWeapon(filteredInventory[itemNumber]);
                            }
                            break;
                        case ConsoleKey.Enter:
                            if (onMainMenu)
                            {
                                menuOpen = false;
                            }  
                            else
                            {
                                filteredInventory = player.Inventory;
                                header = "";
                                ClearMenu();
                                OpenMenu();
                                PrintOptions();
                                onMainMenu = true;
                            }
                            break;
                    }
                }
            }
            ClearMenu();
            game.Go();
        }

        private void PurchaseConfirmation(string purchase)
        {
            int startTop = Top + 8;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(LeftBound + 30, startTop);
            for (int i = 0; i < 50; i++) Console.Write(' ');
            for (int j = 1; j < 12; j++)
            {
                Console.SetCursorPosition(LeftBound + 30, startTop + j);
                Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.Black;
                for (int i = 0; i < 48; i++) Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(' ');
            }
            Console.SetCursorPosition(LeftBound + 30, startTop + 11);
            for (int i = 0; i < 50; i++) Console.Write(' ');
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(LeftBound + 35, startTop + 5);
            Console.Write($" {purchase, -20}");
            Console.SetCursorPosition(LeftBound + 41, startTop + 8);
            Console.Write($"SpaceBar to Equip");

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }
            if (Console.ReadKey().Key == ConsoleKey.Spacebar && newWeapon != null)
            {
                player.EquipWeapon(newWeapon);
                game.Background.DrawStatusBar(player);
            }  

            ClearMenu();
            OpenMenu();
            PrintOptions();
            NavigateMenu();
        }

        private IWeapon CreateNewWeapon()
        {
            int weaponTypeRnd = rnd.Next(1, 4);
            WeaponTypes weaponType;
            int rarityRnd = rnd.Next(1, 11);
            Rarities rarity;
            int magSize;
            int damage;
            double reloadSpeed;

            player.Cash -= 1000;
            game.Background.DrawStatusBar(player);

            if (weaponTypeRnd == 1)
                weaponType = WeaponTypes.ROCKETLAUNCHER;
            else if (weaponTypeRnd == 2)
                weaponType = WeaponTypes.MACHINEGUN;
            else
                weaponType = WeaponTypes.SHOTGUN;

            if (rarityRnd == 10)
                rarity = Rarities.LEGENDARY;
            else if (rarityRnd <= 6)
                rarity = Rarities.COMMON;
            else
                rarity = Rarities.RARE;

            magSize = weaponType == WeaponTypes.MACHINEGUN ? rnd.Next(10, 31) : weaponType == WeaponTypes.SHOTGUN ? rnd.Next(2, 9) : rnd.Next(1, 5);
            damage = rarity == Rarities.LEGENDARY ? rnd.Next(8, 21) : rarity == Rarities.RARE ? rnd.Next(5, 16) : rnd.Next(3, 11);
            reloadSpeed = rarity == Rarities.LEGENDARY ? rnd.Next(500, 2000) : rnd.Next(1000, 2500);

            switch (weaponType)
            {
                case WeaponTypes.MACHINEGUN:
                    return new MachineGun(weaponType, rarity, magSize, damage, reloadSpeed, player, game.Background);
                case WeaponTypes.SHOTGUN:
                    return new Shotgun(weaponType, rarity, magSize, damage, reloadSpeed, player, game.Background);
                case WeaponTypes.ROCKETLAUNCHER:
                    return new RocketLauncher(weaponType, rarity, magSize, damage, reloadSpeed, player, game.Background);
            }

            return null;
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
            Console.SetCursorPosition(GetCenter(header, LeftBound, RightBound), Top + 3);
            Console.Write(header);
            Console.SetCursorPosition(GetCenter(divider, LeftBound, RightBound), Top + 4);
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
            Console.SetCursorPosition(GetCenter(controls, LeftBound, RightBound), Bottom - 3);
            Console.Write(controls);
        }

        public int GetCenter(string myString, int boundOne, int boundTwo)
        {
            return (boundTwo - boundOne) / 2 - myString.Length / 2;
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
