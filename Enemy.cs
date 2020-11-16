using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsolePlatformer
{
    class Enemy
    {
		public int Position { get; private set; }
		public int Bottom { get; private set; }
		private Background background;
		private Player player;
		private Game game;
		public int Health { get; private set; }
		private int speed;
		public int Damage { get; private set; }
		private int frames;
		private string direction;
		private Random rnd = new Random();
		ConsoleColor Color = ConsoleColor.Green;
		public Enemy(int speed, Background background, int position, int bottom, int health, int damage, Player player, Game game)
        {
			this.background = background;
			this.player = player;
			this.game = game;
			this.speed = speed;
			Damage = damage;
			Position = position;
			Bottom = bottom;
			Health = health;
			frames = 0;
        }

		public void MoveRight()
		{
			if (direction != "Right")
				direction = "Right";
		}

		public void MoveLeft()
		{
			if (direction != "Left")
				direction = "Left";
		}

		public void MoveDown()
		{
			if (direction != "Down")
				direction = "Down";
		}

		public void MoveUp()
		{
			if (direction != "Up")
				direction = "Up";
		}

		public void HitTest(Projectile projectile)
		{
			int projectileBottom = projectile.Bottom;
			int projectilePosition = projectile.Position;

			if (projectilePosition == Position || projectilePosition == Position - 1)
				if(projectileBottom == Bottom - 1 || projectileBottom == Bottom)
					TakeDamage(projectile);
		}

		public void TakeDamage(Projectile projectile)
		{
			Health -= projectile.Damage;

			if(Health <= 0)
			{
				DrawEnemy(ConsoleColor.Black, Position, Bottom);
			}
				
		}

		public void TakeDamage()
		{
			Health = 0;
			DrawEnemy(ConsoleColor.Black, Position, Bottom);
		}

		public void Draw()
		{
			//if(Health > 0)
			//{
				frames++;

				if (frames % speed == 0)
				{
					DrawEnemy(ConsoleColor.Black, Position, Bottom);
					switch (direction)
					{
						case ("Left"):
							if (Position > background.LeftWall + 2)
								Position--;
							break;
						case ("Right"):
							if (Position < background.RightWall - 1)
								Position++;
							break;
						case ("Up"):
							if (Bottom > background.TopWall + 2)
								Bottom--;
							break;
						case ("Down"):
							if (Bottom < background.BottomWall - 1)
								Bottom++;
							break;
					}

					if (Bottom != player.Bottom)
					{
						switch (rnd.Next(0, 2))
						{
							case 0:
								if (player.Position > Position)
									MoveRight();
								else
									MoveLeft();
								break;
							case 1:
								if (player.Bottom > Bottom)
									MoveDown();
								else
									MoveUp();
								break;
						}
					}
					else
					{
						if (player.Position > Position)
							MoveRight();
						else
							MoveLeft();
					}
				}
				DrawEnemy(Color, Position, Bottom);
			//}
		}

		public void DrawEnemy(ConsoleColor color, int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = color;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(' ');
			Console.SetCursorPosition(position, bottom - 1);
			Console.Write(color == ConsoleColor.Black ? ' ' : '*');
			Console.SetCursorPosition(position - 1, bottom - 1);
			Console.Write(color == ConsoleColor.Black ? ' ' : '*');
			Console.SetCursorPosition(position - 1, bottom);
			Console.Write(' ');
		}
	}
}
