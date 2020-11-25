using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsolePlatformer
{
    /// <summary>
    /// Class for a new game. Sets up game, then runs the game loop until terminated.
    /// </summary>
    class Game
    {
        private Player player;
        public  Background Background;
        private bool running;
        private List<Enemy> enemies;
        private List<Projectile> projectiles;
        private Random rnd;
        public static int Level;
        private int waves;
        private Stopwatch clock;
        private Stopwatch reloadTimer;
        private Menu menu;
        public Game(Player player, Background background, int loadLevel)
        {
            Background = background;
            this.player = player;
            running = true;
            enemies = new List<Enemy>();
            projectiles = new List<Projectile>();
            Level = loadLevel;
            waves = 1;
            clock = new Stopwatch();
            reloadTimer = new Stopwatch();
            rnd = new Random();
            menu = new Menu(player, this);
        }

        /// <summary>
        /// Contains the main game loop. First it will setup the game, then when a key is pressed
        /// the game loop and timer will start. Terminates on quit or player health <= 0.
        /// </summary>
        public void Go()
        {

            Setup();

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            ClearPrompt();

            clock.Start();

            while (running)
            {
                if (clock.ElapsedMilliseconds % 50 == 0)
                {
                    if (player.CurrentHealth <= 0)
                        running = false;

                    CheckLevelUp();

                    if (clock.ElapsedMilliseconds >= 5000)
                    {
                        SpawnEnemyWave();

                    }

                    DrawProjectiles();

                    foreach (Enemy en in enemies)
                    {
                        if (en.Position < Background.RightWall && en.SpawnTimer.ElapsedMilliseconds >= 500)
                        {
                            en.Draw();
                            player.HitTest(en);
                            CheckProjectileHits(en);
                        }
                    }

                    if (Console.KeyAvailable)
                    {
                        running = CheckKeyPress();
                    }

                    ReloadWeapon();
                    Background.DrawAmmo(player);

                    player.Draw();
                }
            }

            menu.OpenMenu();
            menu.DrawGameResults();
            Console.SetCursorPosition(0, Background.BottomWall - 1);
            if (player.CurrentHealth <= 0)
            {
                player.FullHeal();
                Level = 1;
            }
            SaveAndQuit();
        }

        /// <summary>
        /// Clears prompt that says to press any button to start and replaces with empty spaces
        /// </summary>
        private void ClearPrompt()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(menu.GetCenter("                      ", Background.LeftWall, Background.RightWall), menu.GetCenter(" ", Background.TopWall, Background.BottomWall));
            Console.Write("                      ");
        }

        /// <summary>
        /// Loops through projectiles and checks if they hit an enemy
        /// </summary>
        /// <param name="en"></param>
        private void CheckProjectileHits(Enemy en)
        {
            foreach (Projectile pro in projectiles)
            {
                en.HitTest(pro);
            }
        }

        /// <summary>
        /// Loops through projectiles and draws each one
        /// </summary>
        private void DrawProjectiles()
        {
            foreach (Projectile pro in projectiles)
            {
                pro.Draw();
            }
        }

        /// <summary>
        /// Spawns enemy wave based on level. Will spawn 1 enemy per level up to 5.
        /// Then starts back at 1 enemy but with higher speed and damage.
        /// </summary>
        private void SpawnEnemyWave()
        {
            int enemiesToSpawn = Level % 5 != 0 ? Level % 5 : Level;
            waves++;
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                int position = rnd.Next(Background.LeftWall + 2, Background.RightWall - 1);
                int bottom = rnd.Next(Background.TopWall + 2, Background.BottomWall - 1);
                int speed = rnd.Next(2, 6);
                int health = Level > 5 ? 15 * (Level / 5) + 1 : 15;
                int damage = Level > 5 ? 10 * (Level / 5) + 1 : 10;
                Enemy enemy = new Enemy(speed, Background, position, bottom, health, damage, player, this);
                enemies.Add(enemy);
                enemy.DrawSpawnMarker();
            }
            clock.Restart();
        }

        /// <summary>
        /// Checks how many enemy waves have spawned and ups the level after 5 waves
        /// </summary>
        private void CheckLevelUp()
        {
            if (waves > 5)
            {
                Level++;
                Background.DrawStatusBar(player);
                waves = 1;
            }
        }

        /// <summary>
        /// When a key is pressed during the game loop, this will check which key was pressed 
        /// and start the appropriate action.
        /// </summary>
        /// <returns>bool that tells game whether it should still be running</returns>
        private bool CheckKeyPress()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.RightArrow:
                    player.MoveRight();
                    break;
                case ConsoleKey.LeftArrow:
                    player.MoveLeft();
                    break;
                case ConsoleKey.UpArrow:
                    player.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    player.MoveDown();
                    break;
                case ConsoleKey.Spacebar:
                    if (player.EquipedWeapon.BulletsInMagazine > 0)
                    {
                        List<Projectile> newProjectiles = player.EquipedWeapon.Fire();
                        foreach (Projectile pro in newProjectiles)
                        {
                            projectiles.Add(pro);
                        }
                    }
                    break;
                case ConsoleKey.P:
                    clock.Stop();
                    menu = new Menu(player, this);
                    menu.OpenMenu();
                    menu.PrintOptions();
                    break;
                case ConsoleKey.Q:
                    running = false;
                    break;
            }

            return running;
        }

        /// <summary>
        /// Starts a reload timer when bulletsinmagazine is zero. 
        /// When the timer reaches the equipped weapon reload time, it will reload the weapon.
        /// </summary>
        private void ReloadWeapon()
        {
            if (reloadTimer.ElapsedMilliseconds >= player.EquipedWeapon.ReloadSpeed)
            {
                player.EquipedWeapon.Reload();
                reloadTimer.Reset();
            }
            else if (player.EquipedWeapon.BulletsInMagazine == 0)
            {
                if (!reloadTimer.IsRunning)
                    reloadTimer.Start();
            }
        }

        /// <summary>
        /// Draws the background, player, and start game prompt
        /// </summary>
        private void Setup()
        {
            Console.CursorVisible = false;
            Background.DrawAll(ConsoleColor.DarkBlue, player);
            player.Draw();
            Console.SetCursorPosition(menu.GetCenter("Press Any Key to Start", Background.LeftWall, Background.RightWall), menu.GetCenter(" ", Background.TopWall, Background.BottomWall));
            Console.Write("Press Any Key to Start");
        }

        /// <summary>
        /// When game is quit, this will save the player data to save.txt file
        /// </summary>
        private void SaveAndQuit()
        {
            clock.Reset();
            string file = "Save.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine($"GAMELEVEL:{Level}");
                    writer.WriteLine($"$:{player.Cash}");
                    writer.WriteLine($"HEALTH:{player.CurrentHealth}");
                    writer.WriteLine($"MAXHEALTH:{player.MaxHealth}");
                    foreach(IWeapon weapon in player.Inventory)
                    {
                        writer.WriteLine($"{weapon.Type}:{weapon.Rarity}:{weapon.MagazineSize}:{weapon.Damage}:{weapon.ReloadSpeed}");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Problem writing to file {file}");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
