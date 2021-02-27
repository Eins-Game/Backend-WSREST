using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.GameSession
{
    public class Game
    {
        //Same as LobbyID
        public ulong GameID { get; set; }
        public Lobby GameLobby { get; set; }
        public List<GamePlayer> Players { get; set; } = new List<GamePlayer>();

        //TODO: Replace with card object
        public Stack<string> PlayedCards { get; set; } = new Stack<string>();
        public DateTimeOffset StartDate { get; set; }
    }
}
