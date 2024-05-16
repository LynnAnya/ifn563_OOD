using System;
namespace BoardGameSystem
{
	public class BotPlayer : Player
	{
        private readonly Random rand;
        private readonly List<int> doneRandCells;
		public BotPlayer()
		{
            Name = "robot1";
            Score = 0;
            rand = new Random();
            doneRandCells = new List<int>();
        }
        public override Tuple<int, Pieces>? MakeMove()
        {
            //generate random cell position
            int randCell;
            do
            {
                randCell = rand.Next(1, 10);
            } while (doneRandCells.Contains(randCell));
            doneRandCells.Add(randCell);

            //generate random piece
            Pieces randPiece = (rand.Next(0, 2) == 0) ? Pieces.s : Pieces.o;
            return new Tuple<int, Pieces>(randCell, randPiece);
        }
    }
}

