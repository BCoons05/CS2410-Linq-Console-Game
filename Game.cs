using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsolePlatformer
{
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

        public void Go()
        {

            Setup();

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            clock.Start();

            while (running)
            {
                if(clock.ElapsedMilliseconds % 50 == 0)
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
            if(player.CurrentHealth <= 0)
            {
                player.FullHeal();
                Level = 1;
            }
            SaveAndQuit();
        }

        private void CheckProjectileHits(Enemy en)
        {
            foreach (Projectile pro in projectiles)
            {
                en.HitTest(pro);
            }
        }

        private void DrawProjectiles()
        {
            foreach (Projectile pro in projectiles)
            {
                pro.Draw();
            }
        }

        private void SpawnEnemyWave()
        {
            waves++;
            for (int i = 0; i < Level; i++)
            {
                int position = rnd.Next(Background.LeftWall + 2, Background.RightWall - 1);
                int bottom = rnd.Next(Background.TopWall + 2, Background.BottomWall - 1);
                int speed = rnd.Next(2, 6);
                Enemy enemy = new Enemy(speed, Background, position, bottom, 15, 10, player, this);
                enemies.Add(enemy);
                enemy.DrawSpawnMarker();
            }
            clock.Restart();
        }

        private void CheckLevelUp()
        {
            if (waves > 5)
            {
                Level++;
                Background.DrawStatusBar(player);
                waves = 1;
            }
        }

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
                    SaveAndQuit();
                    break;
            }

            return running;
        }

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
        /// Write 'Press Enter to Start ... ' as title on native background
        /// Draw the board
        /// Position the horses on the track
        /// </summary>
        private void Setup()
        {
            Console.CursorVisible = false;
            Background.DrawAll(ConsoleColor.DarkBlue, player);
            player.Draw();
        }

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
