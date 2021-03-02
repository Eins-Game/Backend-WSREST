using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyCreatorUserChangedEventArgs
    {
        public Player NewCreator { get; set; }
        public Player OldCreator { get; set; }
        public Lobby Lobby { get; set; }
    }
}
