using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            IDictionary<string, string> inventoryDict = new Dictionary<string, string>();
            List<IWeapon> inventory = new List<IWeapon>();

            Background background = new Background(1, 105, 5, 29);

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
                        else 
                        {
                            splitLine = line.Split(':');
                            inventoryDict[splitLine[0]] = splitLine[1];
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

            Player player = new Player(background, background.LeftWall + 3, background.TopWall + 10, Health);

            foreach(KeyValuePair<string, string> pair in inventoryDict)
            {
                switch (pair.Key)
                {
                    case "MACHINEGUN":
                        inventory.Add(new MachineGun(WeaponTypes.MACHINEGUN, GetRarity(pair.Value), player, background));
                        break;
                    case "SHOTGUN":
                        inventory.Add(new MachineGun(WeaponTypes.SHOTGUN, GetRarity(pair.Value), player, background));
                        break;
                    case "ROCKETLAUNCHER":
                        inventory.Add(new MachineGun(WeaponTypes.ROCKETLAUNCHER, GetRarity(pair.Value), player, background));
                        break;
                }
            }

            inventory[0].Equip();
            player.EquipWeapon(inventory[0]);

            Game game = new Game(player, background, inventory);
            game.Go();
        }

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
