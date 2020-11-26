using System;

namespace ConsolePlatformer
{
	/// <summary>
	/// Class to create the game background. Prints walls, health bar, status bar, and controls.
	/// </summary>
    class Background
    {
		public int LeftWall { get; }
		public int RightWall { get; }
		public int BottomWall { get; }
		public int TopWall { get; }

		public Background(int leftBound, int rightBound, int topBound, int bottomBound)
		{
			LeftWall = leftBound;
			RightWall = rightBound;
			TopWall = topBound;
			BottomWall = bottomBound;
		}

		/// <summary>
		/// Draws health bar based on passed player current health and fills empty black spaces up to the max health
		/// </summary>
		/// <param name="player">Player</param>
		public void DrawHealthBar(Player player)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(TopWall - 2, LeftWall + 2);
			Console.Write($"HEALTH: ");
			int i;
			Console.BackgroundColor = player.CurrentHealth > 15 ? ConsoleColor.Green : player.CurrentHealth > 5 ? ConsoleColor.DarkYellow : ConsoleColor.Red;
			for (i = 0; i < player.CurrentHealth; i++) 
			{
				Console.Write(' ');
			}
			if(i < player.MaxHealth)
			{
				Console.BackgroundColor = ConsoleColor.Black;
				while(i < player.MaxHealth)
				{
					Console.Write(' ');
					i++;
				}
			}
		}

		/// <summary>
		/// Draws status bar containing current cash, level, and equipped weapon 
		/// for the passed player object
		/// </summary>
		/// <param name="player">Player</param>
		public void DrawStatusBar(Player player, Game game)
		{
			int maxStringLength = 25;
			string weaponString = $"{player.EquipedWeapon.Rarity} {player.EquipedWeapon.Type}";
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(LeftWall + 2, TopWall - 4);
			Console.Write($"CASH: {player.Cash, -35}ROUND {game.Level, -25} {weaponString}");
			for (int i = weaponString.Length; i <= maxStringLength; i++) Console.Write(' ');
		}

		/// <summary>
		/// Draws current bulletsInMagazine and magSize based on passed player
		/// </summary>
		/// <param name="player">Player</param>
		public void DrawAmmo(Player player)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(LeftWall + (player.MaxHealth), TopWall - 2);
			Console.Write($"{(player.EquipedWeapon.BulletsInMagazine > 0 ? $"{player.EquipedWeapon.BulletsInMagazine, 25}" : $"{"Reloading...", 25}")} / {player.EquipedWeapon.MagazineSize}");
		}

		/// <summary>
		/// Draws outer walls in passed color to set boundaries for the game
		/// </summary>
		/// <param name="color">ConsoleColor</param>
		public void DrawBackground(ConsoleColor color)
		{
			Console.BackgroundColor = color;
			for(int i = LeftWall; i <= RightWall; i++)
			{
				Console.SetCursorPosition(i, TopWall);
				Console.Write(' ');
			}
			for(int i = TopWall + 1; i < BottomWall; i++)
			{
				Console.SetCursorPosition(LeftWall, i);
				Console.Write(' ');
				Console.SetCursorPosition(RightWall, i);
				Console.Write(' ');
			}
			for (int i = LeftWall; i <= RightWall; i++)
			{
				Console.SetCursorPosition(i, BottomWall);
				Console.Write(' ');
			}
		}

		/// <summary>
		/// Draws game controls to the right of the game walls
		/// </summary>
		private void DrawControls()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.SetCursorPosition(RightWall + 2, TopWall - 3);
			Console.Write($"{"Up", +7}");
			Console.SetCursorPosition(RightWall + 2, TopWall - 2);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall - 1);
			Console.Write($"{"Up Arrow",+10}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 1);
			Console.Write($"{"Down",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 2);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 3);
			Console.Write(" Down Arrow");
			Console.SetCursorPosition(RightWall + 2, TopWall + 5);
			Console.Write($"{"Left",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 6);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 7);
			Console.Write(" Left Arrow");
			Console.SetCursorPosition(RightWall + 2, TopWall + 9);
			Console.Write($"{"Right",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 10);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 11);
			Console.Write(" Right Arrow");
			Console.SetCursorPosition(RightWall + 2, TopWall + 13);
			Console.Write($"{"Shoot",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 14);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 15);
			Console.Write("  SpaceBar");
			Console.SetCursorPosition(RightWall + 2, TopWall + 17);
			Console.Write($"{"Menu",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 18);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 19);
			Console.Write($"{'P', +6}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 21);
			Console.Write($"{"Quit",+8}");
			Console.SetCursorPosition(RightWall + 2, TopWall + 22);
			Console.Write("------------");
			Console.SetCursorPosition(RightWall + 2, TopWall + 23);
			Console.Write($"{'Q', +6}");

		}

		/// <summary>
		/// Draws all background methods
		/// </summary>
		/// <param name="color">ConsoleColor for walls</param>
		/// <param name="player">Player to get status from</param>
		public void DrawAll(ConsoleColor color, Player player)
		{
			DrawHealthBar(player);
			DrawAmmo(player);
			DrawBackground(color);
			DrawControls();
		}
	}
}
