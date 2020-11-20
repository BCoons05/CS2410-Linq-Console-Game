using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsolePlatformer
{
    class Game
    {
        private Player player;
        private Background background;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Projectile> projectiles = new List<Projectile>();
        Random rnd = new Random();
        private int projCounter;
        private int enemyCounter;
        public Game(Player player, Background background, IEnumerable<IWeapon> inventory)
        {
            this.background = background;
            this.player = player;
            projCounter = 0;
            enemyCounter = 0;
        }

        public void Go()
        {

            Setup();

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            var clock = new Stopwatch();
            bool running = true;
            clock.Start();

            while (running)
            {
                if(clock.ElapsedMilliseconds % 50 == 0)
                {
                    if (player.CurrentHealth <= 0)
                        running = false;

                    if (clock.ElapsedMilliseconds % 5000 == 0)
                    {
                        Enemy enemy = new Enemy(rnd.Next(2, 6), background, rnd.Next(background.LeftWall + 2, background.RightWall - 1), rnd.Next(background.TopWall + 2, background.BottomWall - 1), 15, 5, player, this);
                        enemies.Add(enemy);
                        enemy.Draw();
                        if (enemies.Count > 20)
                            enemyCounter++;
                    }

                    for (int j = enemies.Count > 20 ? enemyCounter : 0; j < enemies.Count; j++)
                    {
                        if (enemies[j].Health > 0)
                        {
                            enemies[j].Draw();
                            player.HitTest(enemies[j]);
                        }
                        for (int i = projectiles.Count > 20 ? projCounter : 0; i < projectiles.Count; i++)
                        {
                            projectiles[i].Draw();
                            enemies[j].HitTest(projectiles[i]);
                        }
                    }

                    if (Console.KeyAvailable)
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
                                Projectile projectile = player.EquipedWeapon.Fire();
                                projectiles.Add(projectile);
                                if(projectiles.Count > 20)
                                    projCounter++;
                                break;
                            case ConsoleKey.P:
                                clock.Stop();
                                break;
                            case ConsoleKey.Escape:
                                running = false;
                                break;
                        } 
                    }
                    player.Draw();
                }
            }
            clock.Stop();
            clock.Reset();
        }

        /// <summary>
        /// Write 'Press Enter to Start ... ' as title on native background
        /// Draw the board
        /// Position the horses on the track
        /// </summary>
        private void Setup()
        {
            Console.CursorVisible = false;
            background.DrawBackground(ConsoleColor.DarkBlue);
            background.DrawStatusBar(player);
            player.Draw();
        }
    }
}
