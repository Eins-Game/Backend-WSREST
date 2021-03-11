using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.TestEntities
{
    public class Game
    {
        public ulong GameID { get; set; }
        public Stack<Card> CurrentStack { get; set; } = new Stack<Card>();

        //ConnectionIDs, Position (zeroIndex)
        public Dictionary<int, Player> Players { get; set; } = new Dictionary<int, Player>();

        //ConnectionID
        public string CurrentPlayer { get; set; } = default;
        public GameStatus Status { get; set; } = GameStatus.NotInitialized;

        public Game(ulong lobbyID,
            Dictionary<int, Player> players,
            string firstPlayer,
            Card startingCard = default)
        {
            this.GameID = lobbyID;
            this.Players = players;
            this.CurrentPlayer = firstPlayer;
            
            //Implement later
            //this.CurrentStack.Push(startingCard);
        }
    }

    public enum GameStatus
    {
        NotInitialized,
        Initializing,
        Started,
        Ended
    }
}
