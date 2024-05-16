///<summary> Factory method</summary>
using System;
namespace BoardGameSystem
{
	//factory method
	public class BoardGameFactory
	{
		private static BoardGameFactory _instance;
		private static readonly object syncRoot = new object();
		private BoardGameFactory() { }
		public static BoardGameFactory Instance
		{
			get
			{
				lock (syncRoot)
				{
					if(_instance == null)
					{
						_instance = new BoardGameFactory();
					}
					return _instance;
				}
			}
		}

		//focus on building game
		public BoardGame CreateGame(string firstPlayerName, string secondPlayerName, int gameMode)
		{
			return new SosGame(firstPlayerName, secondPlayerName, gameMode);
		}
	}
}

