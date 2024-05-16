using System;
namespace BoardGameSystem
{
	public abstract class Player
	{
        private string name;
		private int score;

		//property
        public string Name
		{
			get { return name; }
			set
			{
                if (string.IsNullOrWhiteSpace(value))
                {
					name = "anonymousName";
                }
				else
				{
                    name = value;
                }
            }
		}
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        //constructor
        public Player(string inputName = "player1")
		{
			Name = inputName;
			Score = 0;
		}
		public Player()
		{
            name = "player1";
            Score = 0;
        }
		//methods
		public abstract Tuple<int, Pieces>? MakeMove();

	}
}

