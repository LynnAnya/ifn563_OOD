using System;
namespace BoardGameSystem
{
    public class Grid
    {
        //fields
        public readonly int row = 3;
        public readonly int col = 3;
        public char[,] board;

        public Grid()
        {
            board = new char[row, col];
            InitializeBoard();
        }
        public void InitializeBoard()
        {
            //create each grid
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    board[i, j] = ' ';
                }
            }
        }
        public void DisplayBoard()
        {
            // show in the game
            for (int i = 0; i < row; ++i)
            {
                for (int j = 0; j < col; ++j)
                {
                    Console.Write(board[i, j]);
                    if (j < col - 1) Console.Write("|");
                }
                Console.WriteLine();
                if (i < row - 1) Console.WriteLine("-----");
            }
        }
        //set piece to board 
        public bool SetPiece(int x, int y, char value)
        {
            if (x >= 0 && x < row && y >= 0 && y < col)
            {
                board[x, y] = value;
                return true;
            }
            return false;
        }
        //gget piece
        public char GetPiece(int x, int y)
        {
            if (x >= 0 && x < row && y >= 0 && y < col)
            {
                return board[x, y];
            }
            else
            {
                throw new ArgumentException("Invalid position");
            }
        }
        //turn 1-9 to the position 
        public Tuple<int, int> TurnNumToPosition(int cell)
        {
            int row = (cell - 1) / 3;
            int col = (cell - 1) % 3;
            return new Tuple<int, int>(row, col);
        }
    }
}
