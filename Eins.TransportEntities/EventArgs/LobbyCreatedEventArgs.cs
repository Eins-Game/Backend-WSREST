using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyCreatedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }

        public ulong LobbyId { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string GameMode { get; set; }
        public string Password { get; set; }
        public int? PlayerCount { get; set; }
        public int? MaxPlayers { get; set; }
    }
}
