using System;
using System.Diagnostics;

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
		public int MaxHealth { get; private set; }
		private int speed;
		public int Damage { get; private set; }
		private int frames;
		private Directions direction;
		private Random rnd = new Random();
		public Stopwatch SpawnTimer;
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
			MaxHealth = health;
			Health = health;
			frames = 0;
			SpawnTimer = new Stopwatch();
		}

		public void MoveRight()
		{
			if (direction != Directions.RIGHT)
				direction = Directions.RIGHT;
		}

		public void MoveLeft()
		{
			if (direction != Directions.LEFT)
				direction = Directions.LEFT;
		}

		public void MoveDown()
		{
			if (direction != Directions.DOWN)
				direction = Directions.DOWN;
		}

		public void MoveUp()
		{
			if (direction != Directions.UP)
				direction = Directions.UP;
		}

		public void HitTest(Projectile projectile)
		{
			int projectileBottom = projectile.Bottom;
			int projectilePosition = projectile.Position;

			if (projectilePosition == Position)
			{
				if (projectileBottom == Bottom - 1 || projectileBottom == Bottom)
				{
					TakeDamage(projectile);
					projectile.Position = background.RightWall + 2;
				}	
			}
				
		}

		public void TakeDamage(Projectile projectile)
		{
			Health -= projectile.Damage;

			if (Health <= 0)
			{
				DrawEnemy(ConsoleColor.Black, Position, Bottom);
				Position = background.RightWall + 2;
				DrawEnemy(ConsoleColor.Black, Position, Bottom);
				player.Cash += MaxHealth;
				background.DrawStatusBar(player);
			}	
		}

		public void TakeDamage()
		{
			Health = 0;
			DrawEnemy(ConsoleColor.Black, Position, Bottom);
			Position = background.RightWall + 2;
			DrawEnemy(ConsoleColor.Black, Position, Bottom);
		}

		public void Draw()
		{
			frames++;
			SpawnTimer.Stop();
			if (frames % speed == 0 && Position < background.RightWall)
			{
				DrawEnemy(ConsoleColor.Black, Position, Bottom);
				switch (direction)
				{
					case (Directions.LEFT):
						if (Position > background.LeftWall + 2)
							Position--;
						break;
					case (Directions.RIGHT):
						if (Position < background.RightWall - 1)
							Position++;
						break;
					case (Directions.UP):
						if (Bottom > background.TopWall + 2)
							Bottom--;
						break;
					case (Directions.DOWN):
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
		}

		internal void DrawSpawnMarker()
		{
			Console.SetCursorPosition(Position - 1, Bottom);
			Console.BackgroundColor = Color;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("**");
			SpawnTimer.Start();
		}

		public void DrawEnemy(ConsoleColor color, int position, int bottom)
		{
			Console.SetCursorPosition(position - 1, bottom - 2);
			Console.BackgroundColor = ConsoleColor.Black;
			if(bottom > background.TopWall + 2 && position < background.RightWall - 1)
			{
				int i;
				Console.ForegroundColor = Health > 10 ? ConsoleColor.Green : Health > 5 ? ConsoleColor.DarkYellow : ConsoleColor.Red;
				Console.SetCursorPosition(Position - 1, Bottom - 2);
				for (i = 0; i < Health; i += 5)
				{
					Console.Write(color == ConsoleColor.Black ? ' ' : '-');
				}
				if (i < MaxHealth)
				{
					while (i < MaxHealth)
					{
						Console.Write(' ');
						i++;
					}
				}
			}
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
