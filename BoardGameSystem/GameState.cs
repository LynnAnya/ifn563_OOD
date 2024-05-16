///<summary> Game state stores state at a given time
///and handles all methods related to state, undo/redo save/load
///</summary>

using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;
namespace BoardGameSystem
{
	public class GameState
	{
		//stack to get the recent one out
		public Stack<GameMoment> undoGame = new Stack<GameMoment>(); 
        public Stack<GameMoment> redoGame = new Stack<GameMoment>();
        //method
        public bool SaveGame(BoardGame game)
        {
            var record = game.CreateGameMoment();  // capture the moment of the game before saving it
            using (StreamWriter writer = new StreamWriter("gameFile.txt"))
            {
                writer.WriteLine(record.BoardState);
                writer.WriteLine(record.CurrentPlayerName);
                writer.WriteLine(record.Player1Score);
                writer.WriteLine(record.Player2Score);
                  // Save the game mode
            }
            if (File.Exists("gameFile.txt"))
            {
                var firstLine = File.ReadLines("gameFile.txt").FirstOrDefault();
                if (firstLine == record.BoardState)
                {
                    WriteLine("Game saved successfully.");
                    return true;
                }
                else
                {
                    WriteLine("Game cannot be saved.");
                    return false;
                }
            }
            else
            {
                WriteLine("Game cannot be saved.");
                return false;
            }
        }

        public bool LoadGame(ref BoardGame game, bool isNewGame)
        {
            if (!File.Exists("gameFile.txt"))
            {
                WriteLine("Saved game file not found");
                return false;
            }
            using (StreamReader reader = new StreamReader("gameFile.txt"))
            {
                string boardState = reader.ReadLine();
                string currentPlayerName = reader.ReadLine();
                string player1Score = reader.ReadLine();
                string player2Score = reader.ReadLine();
                if (string.IsNullOrEmpty(boardState) ||
                    string.IsNullOrEmpty(currentPlayerName) ||
                    string.IsNullOrEmpty(player1Score) ||
                    string.IsNullOrEmpty(player2Score))
                {
                    WriteLine("File is null or empty.");
                    return false;
                }
                if (isNewGame || game == null)
                {
                    game = BoardGameFactory.Instance.CreateGame(currentPlayerName, "Name2", 2);
                }
                game.SetGameMoment(new GameMoment
                {
                    BoardState = boardState,
                    CurrentPlayerName = currentPlayerName,
                    Player1Score = int.Parse(player1Score),
                    Player2Score = int.Parse(player2Score)
                });
            }
            return true;
        }
        public void Undo(BoardGame game)
		{
            if (undoGame.Count > 0)
			{
                var lastRecord = undoGame.Pop(); 
                var previousRecord = undoGame.Pop();
                redoGame.Push(lastRecord); 
                game.SetGameMoment(previousRecord);  
            }
			else
			{
				WriteLine("No move to undo");
			}
		}
		public void Redo(BoardGame game)
		{
			if(redoGame.Count > 0)
			{
                var record = redoGame.Pop();
				
				undoGame.Push(game.CreateGameMoment());
				game.SetGameMoment(record);
            }
            else
            {
                WriteLine("Cannot redo");
            }
        }
		public void AddGameMoment(GameMoment gameMoment)
		{
			//save current state and then can undo it
			undoGame.Push(gameMoment);
			redoGame.Clear();
		}
	}
}

