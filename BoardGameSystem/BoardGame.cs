using System;
using static System.Console;
namespace BoardGameSystem
{
    public abstract class BoardGame
    {
        //some data fields here
        public Grid Board { get; set; }
        public List<Player> Players { get; set; }
        public Player? CurrentPlayer { get; set; }
        public Player? Winner { get; set; }
        public int cell;
        public char piece;
        public int row;
        public int col;
        public Tuple<int, int> NumBoard;
        protected GameState gameState;
        //constructor
        public BoardGame(string FirstplayerName = "noName1", string SecondplayerName = "noName2",  int playerMode = 2)
        {
            Board = new Grid();
            Players = new List<Player>();
            CurrentPlayer = null;  
            Winner = null;
            gameState = new GameState();
          
            // first player[0] is human at all time, is you 
            Players.Add(new HumanPlayer());
            Players[0].Name = FirstplayerName;
            //next player could be either human or robot
            if (playerMode == (int)GameMode.HumanVsHuman)
            {
                Players.Add(new HumanPlayer());
                Players[1].Name = SecondplayerName;
            }
            else
            {
                Players.Add(new BotPlayer());
                Players[1].Name = SecondplayerName;
            }
            CurrentPlayer = Players[0];
        }
        public virtual void PlayGame()
		{
            //template method
			//have sequence high level methods when playing game, too detail will be inside each method
			InitializeGame();
             gameState.undoGame.Push(CreateGameMoment());

            while (!IsBoardFull())
            {
                if (CurrentPlayer != null)
                {
                    WriteLine($"\n>>> {CurrentPlayer.Name}'s turn <<<<");
                }
                else
                {
                    WriteLine("No current player");
                }

                ProcessMove();
                gameState.undoGame.Push(CreateGameMoment());
                gameState.redoGame.Clear();
                InitializeGame();

                // If current player is not a bot, show options.
                if (!(CurrentPlayer is BotPlayer))
                {
                    int input = 0;
                    do
                    {
                        WriteLine("Choose an action: [1] Continue [2] Undo last move, [3] Help, [4] Menu");
                        input = int.Parse(ReadLine());
                    }
                    while (input < 1 || input > 4);

                    switch (input)
                    {
                        case 1: // continue game, do nothing
                            gameState.redoGame.Clear();
                            break;
                        case 2: // Undo last move
                            if (gameState.undoGame.Count > 1)
                            {
                                gameState.redoGame.Push(CreateGameMoment());
                                gameState.Undo(this);
                                InitializeGame();
                                WriteLine($"\n>>> {CurrentPlayer.Name}'s turn <<<<");

                                WriteLine("Would you like to redo the last undone move? (y/n)");
                                char redoChoice = Console.ReadKey().KeyChar;
                                if (redoChoice == 'y' || redoChoice == 'Y')
                                {
                                    gameState.Redo(this);
                                    InitializeGame();
                                    WriteLine("\nRedo performed.");
                                }
                            }
                            else
                            {
                                WriteLine("No more moves to undo. Continue playing.");
                            }
                            break;
                        case 3: // Show Help
                            HelpSystem.ShowHelp();
                            break;
                        case 4: // Display Menu
                            GameUI.Instance.DisplayGameMenu();
                            break;
                    }
                }
                else // If it's a bot player
                {
                    // Auto-continue
                    gameState.redoGame.Clear();
                }

                CheckLastWin();
                SwitchPlayer();
            }

            // Display the result after exiting the loop
            DisplayResult();
          
        }
        protected virtual void InitializeGame()
		{
            //first time ? how about saved one?? -->> handle that later
            //make sure everything ready for play 

            //let's have 2 players by default and if you have more, you can overwrite yourself
            //must have players done right before you come to this play 
            //score + players

            if (Players != null && Players.Count == 2 && Players[0] != null && Players[1] != null)
            {
                WriteLine("\nPlayer 1 >> {0}: {1}   || Player 2 >> {2}: {3}", Players[0].Name, Players[0].Score, Players[1].Name, Players[1].Score); 
                Board.DisplayBoard();
            }
            else
            {
                WriteLine("Player information is incomplete or missing.");
            }   
        }
        protected abstract bool IsBoardFull();
        protected abstract void ProcessMove();
        protected abstract void CheckLastWin();
        protected abstract void SwitchPlayer();
        protected abstract void DisplayResult();
        protected abstract void BacktoMenu(string msg);
        protected abstract void BacktoMenu(string msg, bool forceToMenu);
        public abstract GameMoment CreateGameMoment();
        public abstract void SetGameMoment(GameMoment gameMoment);
    }
}

