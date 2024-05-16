/// <summary> This UI class is like a receptionist in company
/// who is the first point of contact
/// it will validate general info first beofore diecting player (client)
/// to expected classes (department) </summary>

using System;
using static System.Console;
namespace BoardGameSystem
{
	public class GameUI
	{
        public BoardGame Sos;
        private GameState gameState = new GameState();
        private static GameUI _instance;
        private static readonly object syncRoot = new object();
		//constructor
        private GameUI() { }
        //property
        public static GameUI Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new GameUI();
                    }
                    return _instance;
                }
            }
        }
		//methods
		public void DisplayGameMenu()
		{ 
			string InputStr;
			int UserOption;
            bool isValidInput;
            int modeOption = 0;
            WriteLine("\n---------- Welcome to SOS Game ----------\n");
            do
			{
				WriteLine("\n ....Menu....\n");
				WriteLine("  1 - Start new game");
				WriteLine("  2 - Load the game");
				WriteLine("  3 - Save game");
				WriteLine("  4 - Exit game");
				WriteLine("  5 - Help");
				Write("  Select (1 - 5) : ");
				InputStr = ReadLine();
                isValidInput = int.TryParse(InputStr, out UserOption);

                //validate input
                if (!isValidInput || UserOption < 1 || UserOption > 5)
                {
					WriteLine("Invalid input. Please select 1 - 5");
                    continue;
				}
                switch (UserOption)
				{  
                    case 1: //Start new game
                        WriteLine("[No. 1 is selected.]"); 
                        string name1, name2 = "";
                        isValidInput = false;
                   
                        //ask mode of player first 
                        while (!isValidInput)
                        {
                            Write("Game Mode, select [1] human VS Human, [2] Human Vs robot : ");
                            if (int.TryParse(ReadLine(), out modeOption))
                            { 
                                if (modeOption == (int)GameMode.HumanVsHuman || modeOption == (int)GameMode.HumanVsRobot)
                                {
                                    // Successfully parsed
                                    isValidInput = true;  //break loop here
                                }
                                else
                                {
                                    WriteLine("Invalid game mode input. Please enter the right mode number.");
                                } 
                            }
                            else
                            {
                                WriteLine("Invalid input. Please enter a  digit number.");
                            }
                        }
                        //then ask player name
                        Write("Player 1's name: ");
                        name1 = ReadLine();
                        if (string.IsNullOrEmpty(name1))
                        {
                            name1 = "human1";
                        }
                        //if play with another human
                        if (modeOption == (int)GameMode.HumanVsHuman)
                        {
                            Write("Player 2's name: "); 
                            name2 = ReadLine();
                            if (string.IsNullOrEmpty(name2))
                            {
                                name2 = "human2";
                            }
                        }
                        else if (modeOption == (int)GameMode.HumanVsRobot)
                        {
                            name2 = "IamBot";
                        }
                        //if everything done, let's create board with these inputs and go to play
                        Sos = BoardGameFactory.Instance.CreateGame(name1, name2, modeOption);
                        Sos.PlayGame();
                        break;
                    case 2: //load game
                        WriteLine("[No. 2 is selected.]");
                        bool isNewGame = (Sos == null);
                        if (gameState.LoadGame(ref Sos, isNewGame))
                        {
                            Sos.PlayGame();
                        }
                        else
                        {
                            WriteLine("Game cannot be loaded.");
                        }
                        break;
                    case 3: //Save game
                        WriteLine("[No. 3 is selected.]");
                        if (gameState.SaveGame(Sos))
                        {
                            WriteLine("Game is successfully saved.");
                        }
                        else
                        {
                            WriteLine("Game is failed to save");
                        }
                        break;
                    case 4: //Exit game
                        WriteLine("[No. 5 is selected.] : \n" +
                            "Exit game. Thank you see you next time."); 
                        Environment.Exit(1);
                        break;
                    case 5: //help
                        WriteLine("[No. 5 is selected.]");
                        HelpSystem.ShowHelp();
                        break;
                    default:
                        WriteLine("Invalid input, Please select from 1-5 again");
                        break;
                }
            } while (true);
        }
	}
}

