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
        public Dictionary<int, string> Players { get; set; } = new Dictionary<int, string>();

        //ConnectionID
        public string CurrentPlayer { get; set; } = default;
        public string NextPlayer { get; set; }
    }
}
