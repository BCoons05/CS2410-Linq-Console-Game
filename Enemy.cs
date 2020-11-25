using System;
using System.Diagnostics;

namespace ConsolePlatformer
{
	/// <summary>
	/// Class for an enemy object. 
	/// </summary>
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

		/// <summary>
		/// sets enemy direction to Directions.RIGHT if direction is not right.
		/// This can be used to change how the enemy object is rendered on screen. 
		/// </summary>
		public void MoveRight()
		{
			if (direction != Directions.RIGHT)
				direction = Directions.RIGHT;
		}

		/// <summary>
		/// sets enemy direction to Directions.LEFT if direction is not left.
		/// This can be used to change how the enemy object is rendered on screen. 
		/// </summary>
		public void MoveLeft()
		{
			if (direction != Directions.LEFT)
				direction = Directions.LEFT;
		}

		/// <summary>
		/// sets enemy direction to Directions.DOWN if direction is not down.
		/// This can be used to change how the enemy object is rendered on screen. 
		/// </summary>
		public void MoveDown()
		{
			if (direction != Directions.DOWN)
				direction = Directions.DOWN;
		}

		/// <summary>
		/// sets enemy direction to Directions.UP if direction is not up.
		/// This can be used to change how the enemy object is rendered on screen. 
		/// </summary>
		public void MoveUp()
		{
			if (direction != Directions.UP)
				direction = Directions.UP;
		}

		/// <summary>
		/// Checks to see if passed projectile is in the same space on screen as the enemy object.
		/// If the positions are the same, the enemy will TakeDamage from the passed projectile
		/// </summary>
		/// <param name="projectile"></param>
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

		/// <summary>
		/// If enemy object is Hit by Projectile, this will damage the enemy.
		/// If the enemy health is <= 0, enemy will be cleared and moved outside the walls of the game.
		/// Enemies outside the walls will not be drawn or checked for hits
		/// </summary>
		/// <param name="projectile">Projectile</param>
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

		/// <summary>
		/// If enemy is in the same position as the player, the enemy health is 0 and it is moved outside the walls.
		/// Enemies outside the walls will not be drawn or checked for hits
		/// </summary>
		public void TakeDamage()
		{
			Health = 0;
			DrawEnemy(ConsoleColor.Black, Position, Bottom);
			Position = background.RightWall + 2;
			DrawEnemy(ConsoleColor.Black, Position, Bottom);
		}

		/// <summary>
		/// Draws the enemy in the next screen position based on their current direction
		/// and the position in relation to the walls. Enemy cannot move to a spot that is a wall.
		/// </summary>
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

		/// <summary>
		/// Draws the top half of the enemy model. Used to notify player of an incoming enemy. 
		/// Cannot take or deal damage at this step.
		/// </summary>
		internal void DrawSpawnMarker()
		{
			Console.SetCursorPosition(Position - 1, Bottom);
			Console.BackgroundColor = Color;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("**");
			SpawnTimer.Start();
		}

		/// <summary>
		/// Draws an enemy object on the screen using the passed color, x position, and y bottom.
		/// </summary>
		/// <param name="color">ConsoleColor</param>
		/// <param name="position">int x position</param>
		/// <param name="bottom">int y position</param>
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
