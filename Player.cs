using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsolePlatformer
{
	/// <summary>
	/// Enum holds the possible directions for a player, enemy, or projectile
	/// </summary>
	enum Directions { RIGHT, LEFT, UP, DOWN }
	/// <summary>
	/// Class for a player object. Used as the indicator of where the user is on the screen. 
	/// The player object can move based on key presses, and holds and inventory, health, and cash
	/// </summary>
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
		public IList<IWeapon> Inventory { get; private set; }
		public IWeapon EquipedWeapon { get; private set; }
		private ConsoleColor Color = ConsoleColor.Red;
		public Player(Background background, int position, int bottom, int health, int maxHealth, int cash, IList<IWeapon> inventory)
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

		/// <summary>
		/// Equips the passed IWeapon for use by the player in game
		/// </summary>
		/// <param name="weapon">IWeapon</param>
		public void EquipWeapon(IWeapon weapon)
		{
			EquipedWeapon = weapon;
		}

		/// <summary>
		/// If player is facing right the player will be set to moving. 
		/// If direction is not right, the direction will be set to right.
		/// </summary>
		public void MoveRight()
		{
			if (Direction == Directions.RIGHT)
				moving = true;
			else
				Direction = Directions.RIGHT;
		}

		/// <summary>
		/// If player is facing left the player will be set to moving. 
		/// If direction is not left, the direction will be set to left.
		/// </summary>
		public void MoveLeft()
		{
			if (Direction == Directions.LEFT)
				moving = true;
			else
				Direction = Directions.LEFT;
		}

		/// <summary>
		/// If player is facing down the player will be set to moving. 
		/// If direction is not down, the direction will be set to down.
		/// </summary>
		public void MoveDown()
		{
			if (Direction == Directions.DOWN)
				moving = true;
			else
				Direction = Directions.DOWN;
		}

		/// <summary>
		/// If player is facing up the player will be set to moving. 
		/// If direction is not up, the direction will be set to up.
		/// </summary>
		public void MoveUp()
		{
			if (Direction == Directions.UP)
				moving = true;
			else
				Direction = Directions.UP;
		}

		/// <summary>
		/// Checks to see if passed enemy object is in the same position as the player. 
		/// If the position is the same, then the player takes damage
		/// </summary>
		/// <param name="enemy"></param>
		public void HitTest(Enemy enemy)
		{
			int enemyBottom = enemy.Bottom;
			int enemyPosition = enemy.Position;

			if (enemyPosition == Position || enemyPosition == Position - 1)
				if (enemyBottom == Bottom - 1 || enemyBottom == Bottom)
					TakeDamage(enemy);
		}

		/// <summary>
		/// Player will take damage equal to the damage of the enemy object that is passed.
		/// </summary>
		/// <param name="enemy">Enemy</param>
		public void TakeDamage(Enemy enemy)
		{
			CurrentHealth -= enemy.Damage;
			background.DrawHealthBar(this);
			enemy.TakeDamage();
		}

		/// <summary>
		/// If the user purchases health in the menu, This will increase the maxHealth and currentHealth
		/// by 10 and then will decrease player cash by 1000
		/// </summary>
		public void UpgradeHealth(Game game)
		{
			MaxHealth += 10;
			CurrentHealth += 10;
			Cash -= 1000;
			background.DrawStatusBar(this, game);
		}

		/// <summary>
		/// Used to set CurrentHealth to be equal to MaxHealth of player
		/// </summary>
		public void FullHeal()
		{
			CurrentHealth = MaxHealth;
		}

		/// <summary>
		/// Moves player on screen based on the direction of the player and whether they are moving or not.
		/// </summary>
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
						if (Bottom > background.TopWall + 2)
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

		/// <summary>
		/// Draws player model on screen. Uses direction to decide where to draw the eyes.
		/// Player model is drawn where the position and bottom are on screen.
		/// </summary>
		/// <param name="color">ConsoleColor for color of player model</param>
		/// <param name="position">int for x position</param>
		/// <param name="bottom">int for y bottom</param>
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
