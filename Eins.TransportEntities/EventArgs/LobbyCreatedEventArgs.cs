using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyCreatedEventArgs
    {
        public Lobby Lobby { get; set; }
        public string Password { get; set; }
    }
}
