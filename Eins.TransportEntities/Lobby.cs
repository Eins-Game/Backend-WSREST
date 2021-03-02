using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Eins.TransportEntities
{
    public class Lobby
    {
        public ulong SessionID { get; set; }
        public string Password { get; set; } = "";
        public Player LobbyCreator { get; set; }

        public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;

        //TODO: Replace with CustomGameSettings object
        public string GameMode { get; set; }
        public bool GameInProgress { get; set; } = false;

        public ConcurrentDictionary<string, Player> Players { get; set; } = new ConcurrentDictionary<string, Player>();

    }
}