using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsolePlatformer
{
    class Menu
    {
        private Game game;
        private Player player;
        private List<IWeapon> inventory;
        private Dictionary<int, IWeapon> weaponDict;
        private int LeftBound;
        private int RightBound;
        private int Top;
        private int Bottom;
        private bool menuOpen;

        public Menu(Player player, List<IWeapon> inventory, Game game)
        {
            this.game = game;
            this.player = player;
            this.inventory = inventory;
            weaponDict = new Dictionary<int, IWeapon>();
            Top = 3;
            LeftBound = 3;
            Bottom = 28;
            RightBound = 111;
            menuOpen = true;
            OpenMenu();
            PrintOptions();
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
            Console.SetCursorPosition(50, Top + 3);
            Console.Write("Inventory");
            Console.SetCursorPosition(49, Top + 4);
            Console.Write("-----------");
            Console.SetCursorPosition(LeftBound + 1, Top + 6);
            Console.Write($"{' ', -20}M - Machine Guns{' ', -25}C - Common Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 11);
            Console.Write($"{' ', -20}S - Shotguns{' ', -29}R - Rare Weapons");
            Console.SetCursorPosition(LeftBound + 1, Top + 16);
            Console.Write($"{' ', -20}B - Rocket Launchers {' ',-20}L - Legendary Weapons");
            Console.SetCursorPosition(LeftBound + 1, Bottom - 6);
            Console.Write($"{' ',-31}O - Purchase Random Weapon for $1000");
            Console.SetCursorPosition(LeftBound + 1, Bottom - 3);
            Console.Write($"{' ',-40}Enter to Close Menu");

            NavigateMenu();
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
            IEnumerable<IWeapon> legendaries =
                from w in inventory
                where w.Rarity == Rarities.LEGENDARY
                select w;

            OpenMenu();
            UpdateMenuSelections(legendaries, "Legendary");
        }

        private void UpdateMenuSelections(IEnumerable<IWeapon> filteredWeapons, string header)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(50, Top + 3);
            Console.Write(header);
            Console.SetCursorPosition(49, Top + 4);
            Console.Write("-----------\n");
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
            Console.SetCursorPosition(30, Bottom - 3);
            Console.Write("Up/Down to Select -- SpaceBar to Equip -- Enter to Go Back");
        }

        private void GetLaunchers()
        {
            IEnumerable<IWeapon> launchers =
                from w in inventory
                where w.Type == WeaponTypes.ROCKETLAUNCHER
                select w;

            OpenMenu();
            UpdateMenuSelections(launchers, "Rocket Launchers");
        }

        private void GetRares()
        {
            IEnumerable<IWeapon> rares =
                from w in inventory
                where w.Rarity == Rarities.RARE
                select w;

            OpenMenu();
            UpdateMenuSelections(rares, "Rare");
        }

        private void GetShotguns()
        {
            IEnumerable<IWeapon> shotguns =
                from w in inventory
                where w.Type == WeaponTypes.SHOTGUN
                select w;

            OpenMenu();
            UpdateMenuSelections(shotguns, "Shotguns");
        }

        private void GetCommons()
        {
            IEnumerable<IWeapon> commons =
                from w in inventory
                where w.Rarity == Rarities.COMMON
                select w;

            OpenMenu();
            UpdateMenuSelections(commons, "Common");
        }

        private void GetMachineGuns()
        {
            IEnumerable<IWeapon> machineGuns =
                from w in inventory
                where w.Type == WeaponTypes.MACHINEGUN
                select w;

            OpenMenu();
            UpdateMenuSelections(machineGuns, "Machine Guns");
        }
    }
}
