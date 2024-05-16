using System;
using Microsoft.VisualBasic;
using System.Text;
using static System.Console;
namespace BoardGameSystem
{
    public class SosGame : BoardGame
    {
        public SosGame(string firstPlayerName, string secondPlayerName, int gameMode)
                : base(firstPlayerName, secondPlayerName, gameMode) { }
        //methods
        protected override bool IsBoardFull()
        {
            //if board full, game done 
            for (int i = 0; i < Board.row; ++i)
            {
                for (int j = 0; j < Board.col; ++j)
                {
                    if (Board.board[i, j] == ' ')
                    {
                        return false; // the board is not full
                    }
                }
            }
            return true;
        }
        protected override void ProcessMove()
        {
            //system validates move 
            //the board check if win that match or have SOS with that move ??
            if (CurrentPlayer == null)
            {
                BacktoMenu("No current player", true);
                return;
            }
            bool isCorrectPosition = false;
            while (!isCorrectPosition)
            {
                //prompt the current player makes the move, should validate the piece
                var moveReturns = CurrentPlayer.MakeMove();
                //loop through each cell -> process next
                if (moveReturns == null)
                {
                    BacktoMenu("Something's wrong, please try again");
                    continue;
                }
                cell = moveReturns.Item1;
                piece = (char)moveReturns.Item2;
                NumBoard = Board.TurnNumToPosition(cell);
                row = NumBoard.Item1;
                col = NumBoard.Item2;

                //is the cell position taken? if not, place piece
                if (Board.board[row, col] == ' ')
                {
                    if (Board.SetPiece(row, col, piece))
                    {
                        isCorrectPosition = true;
                    }
                    else
                    {
                        BacktoMenu("Something's wrong with placing this position");
                    }
                }
                else
                {    // Bot makes move until it is correct.
                    if (CurrentPlayer is BotPlayer)
                    {
                        CurrentPlayer.MakeMove();
                    }
                    else
                    {
                        WriteLine("This position is already taken, Choose another one.");
                    }
                }
            }
        }
        protected override void CheckLastWin()
        {
            //nobody knows who is the last win yet
            Winner = null;
            bool sosFound = false;
            //check the piece in different angle if matches
            //check horizontal 
            if (Board.board[row, 0] == (char)Pieces.s && Board.board[row, 1] == (char)Pieces.o && Board.board[row, 2] == (char)Pieces.s)
            {
                sosFound = true;
                CurrentPlayer.Score++;
            }
            //check vertical
            if (!sosFound)
            {
                if (Board.board[0, col] == (char)Pieces.s && Board.board[1, col] == (char)Pieces.o && Board.board[2, col] == (char)Pieces.s)
                {
                    sosFound = true;
                    CurrentPlayer.Score++;
                }
            }
            //check diagonal topLeft -> btmRight
            if (!sosFound && row == col)
            {
                if (Board.board[0, 0] == (char)Pieces.s && Board.board[1, 1] == (char)Pieces.o && Board.board[2, 2] == (char)Pieces.s)
                {
                    sosFound = true;
                    CurrentPlayer.Score++;
                }
            }
            //check diagonal btmLeft -> topRight
            if (!sosFound && (row + col == 2))
            {
                if (Board.board[2, 0] == (char)Pieces.s && Board.board[1, 1] == (char)Pieces.o && Board.board[0, 2] == (char)Pieces.s)
                {
                    sosFound = true;
                    CurrentPlayer.Score++;
                }
            }
            if (sosFound)
            {
                // if so update winner equal to currrentplayer, so that player can continue the next turn
                Winner = CurrentPlayer;
            }
        }
        protected override void SwitchPlayer()
        {
            //if the current player can make SOS within that match, they are still in 
            if (CurrentPlayer == Winner)
            {
                WriteLine("Get another turn -- {0}", Winner);
                return;
            }
            else
            {
                if (CurrentPlayer == Players[0])
                {
                    CurrentPlayer = Players[1];
                }
                else
                {
                    CurrentPlayer = Players[0];
                }
            }
        }
        protected override void DisplayResult()
        {
            int compareScore = Players[0].Score.CompareTo(Players[1].Score);
            if (compareScore > 0)
            {
                Winner = Players[0];
            }
            else if (compareScore < 0)
            {
                Winner = Players[1];
            }
            else
            {
                Winner = null;
            }
            WriteLine("         ------ Game Over ------         ");
            if (Winner == null)
            {
                WriteLine("\nThe game is a draw.");
            }
            else
            {
                WriteLine("|--------------------------------------------|");
                WriteLine("\n|>>> The winner is {0} with score {1} <<<       |", Winner.Name, Winner.Score);
                WriteLine("|____________________________________________|");
            }
        }
        protected override void BacktoMenu(string msg = " ")
        {
            WriteLine(msg);
            WriteLine("Press 'm' to return to the main menu or any other key to continue.");
            var input = ReadKey();
            if (input.Key == ConsoleKey.M)
            {
                GameUI.Instance.DisplayGameMenu();
            }
            else
            {
                return;
            }
        }
        protected override void BacktoMenu(string msg, bool forceToMenu)
        {
            if (forceToMenu)
            {
                WriteLine(msg);
                GameUI.Instance.DisplayGameMenu();
            }
        }
        //get the current shot of the game state in time
        public override GameMoment CreateGameMoment()
        {
            string boardStr = "";
            for (int i = 0; i < Board.row; ++i)
            {
                for (int j = 0; j < Board.col; ++j)
                {
                    boardStr += Board.board[i, j];
                    if (j < Board.col - 1)
                    {
                        boardStr += ',';
                    }
                }
                if (i < Board.row - 1)
                {
                    boardStr += '|';
                }
            }
            GameMode currentMode = GameMode.HumanVsHuman;

            if (Players[0] is HumanPlayer && Players[1] is HumanPlayer)
            {
                currentMode = GameMode.HumanVsHuman;
            }
            else if ((Players[0] is HumanPlayer && Players[1] is BotPlayer) ||
                     (Players[0] is BotPlayer && Players[1] is HumanPlayer))
            {
                currentMode = GameMode.HumanVsRobot;
            }
            return new GameMoment   
            {
                BoardState = boardStr,  
                CurrentPlayerName = CurrentPlayer.Name,
                Player1Score = Players[0].Score,
                Player2Score = Players[1].Score,
            };
        }
        //set up the state that is already stored 
        public override void SetGameMoment(GameMoment gameMoment)
        {
            // Split by rows
            string[] rows = gameMoment.BoardState.Split('|');

            // Validate the board dimensions
            if (rows.Length != Board.row)
            {
                throw new Exception("The saved game state's row count is incompatible with the current board dimensions.");
            }

            for (int i = 0; i < rows.Length; i++)
            {
                // split each row by columns
                string[] cells = rows[i].Split(',');

                // Validate column count
                if (cells.Length != Board.col)
                {
                    throw new Exception("The saved game state's column count is incompatible with the current board dimensions.");
                }

                for (int j = 0; j < cells.Length; j++)
                {
                    // Place the piece on the board at the ith row and jth column
                    Board.SetPiece(i, j, cells[j][0]);  // Call SetPiece through Board
                }
            }
        }

    }
} 


