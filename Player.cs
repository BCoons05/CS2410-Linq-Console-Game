using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsolePlatformer
{
    class Player
    {
		private Background background;
		public int Position { get; private set; }
		public string Direction { get; private set; }
		private bool moving;
		public int Bottom { get; private set; }
		public int MaxHealth { get; private set; }
		public int CurrentHealth { get; private set; }
		private ConsoleColor Color = ConsoleColor.Red;
		public Player(Background background, int position, int bottom, int health)
		{
			this.background = background;
			Position = position;
			Bottom = bottom;
			MaxHealth = health;
			CurrentHealth = health;
			Direction = "Right";
		}

		public void MoveRight()
		{
			if (Direction == "Right")
				moving = true;
			else
				Direction = "Right";
		}

		public void MoveLeft()
		{
			if (Direction == "Left")
				moving = true;
			else
				Direction = "Left";
		}

		public void MoveDown()
		{
			if (Direction == "Down")
				moving = true;
			else
				Direction = "Down";
		}

		public void MoveUp()
		{
			if (Direction == "Up")
				moving = true;
			else
				Direction = "Up";
		}

		public void HitTest(Enemy enemy)
		{
			int enemyBottom = enemy.Bottom;
			int enemyPosition = enemy.Position;

			if (enemyPosition == Position || enemyPosition == Position - 1)
				if (enemyBottom == Bottom - 1 || enemyBottom == Bottom)
					TakeDamage(enemy);
		}

		public void TakeDamage(Enemy enemy)
		{
			CurrentHealth -= enemy.Damage;
			background.DrawStatusBar(this);
			enemy.TakeDamage();

			if (CurrentHealth <= 0)
			{
				DrawPlayer(ConsoleColor.Black, Position, Bottom);
			}
		}

		public void Draw()
		{
			DrawPlayer(ConsoleColor.Black, Position, Bottom);

			if (moving)
			{
				switch (Direction)
				{
					case ("Left"):
						if(Position > background.LeftWall + 2)
							Position--;
						break;
					case ("Right"):
						if (Position < background.RightWall - 1)
							Position++;
						break;
					case ("Up"):
						if (Bottom > background.TopWall + 3)
							Bottom--;
						break;
					case ("Down"):
						if (Bottom < background.BottomWall - 1)
							Bottom++;
						break;
				}
			}
			moving = false;
			DrawPlayer(Color, Position, Bottom);
		}

		public void DrawPlayer(ConsoleColor color, int position, int bottom)
		{
			Console.SetCursorPosition(position, bottom);
			Console.BackgroundColor = color;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Write(' ');
			Console.SetCursorPosition(position - 1, bottom);
			Console.Write(' ');
			Console.SetCursorPosition(position, bottom - 1);
			if (color == ConsoleColor.Black)
			{
				Console.BackgroundColor = color;
				Console.Write(' ');
				Console.SetCursorPosition(position - 1, bottom - 1);
				Console.Write(' ');
			}	
			else
			{
				switch (Direction)
				{
					case ("Left"):
						Console.BackgroundColor = color;
						Console.Write(' ');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write('*');
						break;
					case ("Right"):
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write('*');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.BackgroundColor = color;
						Console.Write(' ');
						break;
					case ("Up"):
						Console.BackgroundColor = color;
						Console.Write(' ');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.Write(' ');
						break;
					case ("Down"):
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write('*');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.Write('*');
						break;
				}
			}
		}
	}
}
