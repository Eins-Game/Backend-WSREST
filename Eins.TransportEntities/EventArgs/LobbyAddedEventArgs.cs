using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyAddedEventArgs
    {
        public Lobby Lobby { get; set; }
        public Player Creator { get; set; }
    }
}
