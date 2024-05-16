///<summary>apply Memento pattern to store the game shot</summary>

using System;
namespace BoardGameSystem
{
    public class GameMoment
    {
        public string BoardState { get; set; }
        public string CurrentPlayerName { get; set; }
        public int Player1Score { get; set; }
        public int Player2Score { get; set; }
    }
}

