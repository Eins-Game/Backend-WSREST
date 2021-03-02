using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class UserJoinedEventAgs
    {
        public Lobby Lobby { get; set; }
        public Player Player { get; set; }
    }
}
