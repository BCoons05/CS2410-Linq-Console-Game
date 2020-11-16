using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
    class Projectile
    {
		public int Position { get; private set; }
		public int Bottom { get; private set; }
		private string Direction;
		public int Damage { get; private set; }
		private int Speed;
		private int frames;
		private Background background;
		private ConsoleColor Color = ConsoleColor.Black;
		public ConsoleColor TextColor { get; set; }
		public Projectile(int position, int bottom, string direction, int speed, int damage, Background background)
		{
			Speed = speed;
			Damage = damage;
			Position = position;
			Bottom = bottom;
			Direction = direction;
			frames = 0;
			this.background = background;
			TextColor = ConsoleColor.Yellow;
		}

		public void Draw()
		{
			frames++;
			if (frames % Speed == 0)
			{
				ClearProjectile(ConsoleColor.Black, Position, Bottom);
				if (Position > background.LeftWall + 1 && Position < background.RightWall - 1 && Bottom > background.TopWall + 1 && Bottom < background.BottomWall - 1)
				{
					switch (Direction)
					{
						case "Right":
							DrawProjectile(Color, ++Position, Bottom);
							break;
						case "Left":
							DrawProjectile(Color, --Position, Bottom);
							break;
						case "Up":
							DrawProjectile(Color, Position, --Bottom);
							break;
						case "Down":
							DrawProjectile(Color, Position, ++Bottom);
							break;
					}
				}
			}
		}

		public void DrawProjectile(ConsoleColor color, int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = color;
			Console.ForegroundColor = TextColor;
			if (Direction == "Up" || Direction == "Down")
				Console.Write('|');
			else if (Direction == "Right" || Direction == "Left")
				Console.Write('-');
		}

		public void ClearProjectile(ConsoleColor color, int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = color;
			Console.Write(' ');
			Console.SetCursorPosition(background.RightWall + 1, Bottom);
		}
	}
}
