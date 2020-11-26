using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
	/// <summary>
	/// Enum for types of projectiles
	/// </summary>
	enum ProjectileType { BULLET, PELLET, ROCKET }
	/// <summary>
	/// Projectile class to create a new projectile on screen that starts are a weapon
	/// and moves across the screen in the given direction.
	/// </summary>
    class Projectile
    {
		public ProjectileType Type;
		public int Position { get; set; }
		public int Bottom { get; private set; }
		private Directions Direction;
		public int Damage { get; private set; }
		private int Speed;
		private int frames;
		private Background background;
		private ConsoleColor Color = ConsoleColor.Black;
		public ConsoleColor TextColor { get; }
		private int distance;
		public Projectile(ProjectileType type, int position, int bottom, Directions direction, int speed, int damage, ConsoleColor color, Background background)
		{
			Type = type;
			Speed = speed;
			Damage = damage;
			Position = position;
			Bottom = bottom;
			Direction = direction;
			frames = 0;
			this.background = background;
			distance = type == ProjectileType.PELLET ? 12 : 100;
			TextColor = color;
		}

		/// <summary>
		/// Checks if the projectile is within the walls and if enough frames have passed. If frames have passed 
		/// then the draw method is called. Frames are used to set projectile speed.
		/// </summary>
		public void Draw()
		{
			frames++;
			if (frames % Speed == 0)
			{
				distance--;
				ClearProjectile(Position, Bottom);
				if (distance > 0 && Position > background.LeftWall + 1 && Position < background.RightWall - 1 && Bottom > background.TopWall + 1 && Bottom < background.BottomWall - 1)
				{
					switch (Direction)
					{
						case Directions.RIGHT:
							DrawProjectile(++Position, Bottom);
							break;
						case Directions.LEFT:
							DrawProjectile(--Position, Bottom);
							break;
						case Directions.UP:
							DrawProjectile(Position, --Bottom);
							break;
						case Directions.DOWN:
							DrawProjectile(Position, ++Bottom);
							break;
					}
				}
				else if (distance <= 0)
					Position = background.RightWall + 2;
			}
		}

		/// <summary>
		/// Draws projectile on screen in the given color at the given position and bottom.
		/// draws a | if direction is up or down. Draws - if left or right
		/// </summary>
		/// <param name="color">ConsoleColor of the projectile</param>
		/// <param name="position">int x position</param>
		/// <param name="bottom">int y position</param>
		public void DrawProjectile(int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = Color;
			Console.ForegroundColor = TextColor;
			if (Direction == Directions.UP || Direction == Directions.DOWN)
				Console.Write(Type == ProjectileType.BULLET ? "|" : Type == ProjectileType.ROCKET ? "*" : "\"");
			else if (Direction == Directions.RIGHT || Direction == Directions.LEFT)
				Console.Write(Type == ProjectileType.BULLET ? "-" : Type == ProjectileType.ROCKET ? "*" : ":");
		}

		/// <summary>
		/// Rocket explodes, spawning 4 new projectiles
		/// </summary>
		/// <returns></returns>
		public List<Projectile> Explode()
		{
			List<Projectile> shots = new List<Projectile>();
			Projectile projectile1 = new Projectile(ProjectileType.PELLET, Position, Bottom - 3, Directions.DOWN, 5, Damage, TextColor, background);
			Projectile projectile2 = new Projectile(ProjectileType.PELLET, Position, Bottom + 3, Directions.UP, 5, Damage, TextColor, background);
			Projectile projectile3 = new Projectile(ProjectileType.PELLET, Position + 3, Bottom, Directions.RIGHT, 5, Damage, TextColor, background);
			Projectile projectile4 = new Projectile(ProjectileType.PELLET, Position - 3, Bottom, Directions.LEFT, 5, Damage, TextColor, background);
			shots.Add(projectile1);
			shots.Add(projectile2);
			shots.Add(projectile3);
			shots.Add(projectile4);
			return shots;
		}

		/// <summary>
		/// Draws a space in the place of the projectile. Used to clear old drawings of the projectile
		/// </summary>
		/// <param name="color"></param>
		/// <param name="position"></param>
		/// <param name="bottom"></param>
		public void ClearProjectile(int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = Color;
			Console.Write(' ');
			Console.SetCursorPosition(background.RightWall + 1, Bottom);
		}
	}
}
