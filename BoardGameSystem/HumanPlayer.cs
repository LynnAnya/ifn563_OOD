using System;
using static System.Console;
namespace BoardGameSystem
{
	public class HumanPlayer : Player 
	{
        public override Tuple<int, Pieces>? MakeMove()
        {
			// make sure int and Piece sent to process, validate
			// if position is free to put piece or not should be for the process method loop until right move positon
			string input; 
			while (true)
			{
				Write("Enter the number from 1 - 9 to place the piece: ");
				 input = ReadLine();
				if(input == null)
				{
					WriteLine("Something'wrong. Go back to main menu", true);
                    GameUI.Instance.DisplayGameMenu();
                    return null;
				}
				if (int.TryParse(input, out int cell) && cell >= 1 && cell <= 9)
				{
					//while (true)
					//{
                    Write("Enter the piece,'s' or 'o': ");
                    input = ReadLine();
                    if (Enum.TryParse(input, true, out Pieces piece) && (piece == Pieces.s || piece == Pieces.o))
                    {
                        return new Tuple<int, Pieces>(cell, piece);
                    }
                    else
                    {	
						WriteLine("Invalid piece. It has to be either 's' or 'o' \nEnter 'm' back to menu or any key to continu");
                        input = ReadLine();
						if (input.ToLower() == "m")
						{
                            GameUI.Instance.DisplayGameMenu();
                        }
                        //}
                    }        
                }
				else
				{
					WriteLine("Invalid input of the position for the piece \nEnter 'm' back to menu or any key to continu");
                    input = ReadLine();
                    if (input.ToLower() == "m")
                    {
                        GameUI.Instance.DisplayGameMenu();
						return null;
                    }
                }
            }
		}
	}
}

