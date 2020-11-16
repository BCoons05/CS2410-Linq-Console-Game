using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolePlatformer
{
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

		public void DrawStatusBar(Player player)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			Console.SetCursorPosition(TopWall - 2, LeftWall + 2);
			Console.Write($"HEALTH: ");
			int i;
			Console.BackgroundColor = player.CurrentHealth > 15 ? ConsoleColor.Green : player.CurrentHealth > 5 ? ConsoleColor.Yellow : ConsoleColor.Red;
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
	}
}
