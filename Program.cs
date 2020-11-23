using System;
using System.Collections.Generic;
using System.IO;

namespace ConsolePlatformer
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "Save.txt";
            int GameLevel = 1;
            int Health = 25;
            int MaxHealth = 25;
            int Cash = 0;
            List<string[]> inventoryList = new List<string[]>();
            List<IWeapon> inventory = new List<IWeapon>();

            Background background = new Background(1, 105, 5, 29);

            //Load Saved Data
            try
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] splitLine; 
                        if (line.Contains("GAMELEVEL"))
                        {
                            splitLine = line.Split(':');
                            GameLevel = int.Parse(splitLine[1]);              
                        }
                        else if (line.Contains("HEALTH"))
                        {
                            splitLine = line.Split(':');
                            Health = int.Parse(splitLine[1]);
                        }
                        else if (line.Contains("MAXHEALTH"))
                        {
                            splitLine = line.Split(':');
                            MaxHealth = int.Parse(splitLine[1]);
                        }
                        else if (line.Contains('$'))
                        {
                            splitLine = line.Split(':');
                            Cash = int.Parse(splitLine[1]);
                        }
                        else 
                        {
                            splitLine = line.Split(':');
                            string[] newItem = { splitLine[0], splitLine[1] };
                            inventoryList.Add(newItem);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Problem reading from file {file}");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Player player = new Player(background, background.LeftWall + 3, background.TopWall + 10, Health, MaxHealth, Cash, inventory);

            //Create inventory from dictionary
            foreach(string[] pair in inventoryList)
            {
                switch (pair[0])
                {
                    case "MACHINEGUN":
                        inventory.Add(new MachineGun(WeaponTypes.MACHINEGUN, GetRarity(pair[1]), player, background));
                        break;
                    case "SHOTGUN":
                        inventory.Add(new Shotgun(WeaponTypes.SHOTGUN, GetRarity(pair[1]), player, background));
                        break;
                    case "ROCKETLAUNCHER":
                        inventory.Add(new RocketLauncher(WeaponTypes.ROCKETLAUNCHER, GetRarity(pair[1]), player, background));
                        break;
                }
            }

            inventory[0].Equip();
            player.EquipWeapon(inventory[0]);

            Game game = new Game(player, background, GameLevel);
            game.Go();
        }

        /// <summary>
        /// Gets weapon rarity from string value sent from saved data
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public static Rarities GetRarity(string rarity)
        {
            switch (rarity)
            {
                case "COMMON":
                    return Rarities.COMMON;
                case "RARE":
                    return Rarities.RARE;
                case "LEGENDARY":
                    return Rarities.LEGENDARY;
                default:
                    return Rarities.COMMON;
            }
        }
    }
}
