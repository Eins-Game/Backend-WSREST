using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbiesRequestedEventArgs
    {
        public object LobbyOptions { get; set; }
        public List<Lobby> Lobbies { get; set; }
    }
}
