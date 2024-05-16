using System;
using static System.Console;
namespace BoardGameSystem
{
	public static class HelpSystem
	{
		public static void ShowHelp()
		{
			WriteLine("\n=======================================");
            WriteLine("     Welcome to SOS Game Help Center   ");
			WriteLine("=======================================\n");
            WriteLine("1. Objective of the game >> Trying to create SOS pattern in the grid.\n");
            WriteLine("2. How to play? >>> 2 Players take turn to place either [s] or [o] to the grid board. " +
                "\nIf player can form SOS sequence, they will take the next turn.\n ");
            WriteLine("3. How to win? >>> Player can form the most SOS in the board when the board is filled up " +
                "\nwhether vertically, horizonlly, or diagonally. \n");
            WriteLine("4. How to count score? >>> Each SOS that the player can form will be count as 1 point.\n  ");
            WriteLine("Press any key to return to the main menu. ");
            ReadKey();
        }
	}
}

