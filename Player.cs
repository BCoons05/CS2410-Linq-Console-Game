using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsolePlatformer
{
	enum Directions { RIGHT, LEFT, UP, DOWN }
    class Player
    {
		private Background background;
		public int Position { get; private set; }
		public Directions Direction { get; private set; }
		private bool moving;
		public int Bottom { get; private set; }
		public int MaxHealth { get; private set; }
		public int CurrentHealth { get; private set; }
		public int Cash { get; set; }
		public List<IWeapon> Inventory { get; private set; }
		public IWeapon EquipedWeapon { get; private set; }
		private ConsoleColor Color = ConsoleColor.Red;
		public Player(Background background, int position, int bottom, int health, int maxHealth, int cash, List<IWeapon> inventory)
		{
			this.background = background;
			Position = position;
			Bottom = bottom;
			MaxHealth = maxHealth;
			CurrentHealth = health;
			Cash = cash;
			Direction = Directions.DOWN;
			Inventory = inventory;
		}

		public void EquipWeapon(IWeapon weapon)
		{
			EquipedWeapon = weapon;
		}

		public void MoveRight()
		{
			if (Direction == Directions.RIGHT)
				moving = true;
			else
				Direction = Directions.RIGHT;
		}

		public void MoveLeft()
		{
			if (Direction == Directions.LEFT)
				moving = true;
			else
				Direction = Directions.LEFT;
		}

		public void MoveDown()
		{
			if (Direction == Directions.DOWN)
				moving = true;
			else
				Direction = Directions.DOWN;
		}

		public void MoveUp()
		{
			if (Direction == Directions.UP)
				moving = true;
			else
				Direction = Directions.UP;
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
			background.DrawHealthBar(this);
			enemy.TakeDamage();
		}

		public void Draw()
		{
			DrawPlayer(ConsoleColor.Black, Position, Bottom);

			if (moving)
			{
				switch (Direction)
				{
					case (Directions.LEFT):
						if(Position > background.LeftWall + 2)
							Position--;
						break;
					case (Directions.RIGHT):
						if (Position < background.RightWall - 1)
							Position++;
						break;
					case (Directions.UP):
						if (Bottom > background.TopWall + 3)
							Bottom--;
						break;
					case (Directions.DOWN):
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
					case (Directions.LEFT):
						Console.BackgroundColor = color;
						Console.Write(' ');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write('*');
						break;
					case (Directions.RIGHT):
						Console.BackgroundColor = ConsoleColor.DarkYellow;
						Console.Write('*');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.BackgroundColor = color;
						Console.Write(' ');
						break;
					case (Directions.UP):
						Console.BackgroundColor = color;
						Console.Write(' ');
						Console.SetCursorPosition(position - 1, bottom - 1);
						Console.Write(' ');
						break;
					case (Directions.DOWN):
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
