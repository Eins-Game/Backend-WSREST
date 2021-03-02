using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class JoinGameLobbyExceptionEventArgs
    {
        public string ConnectionID { get; set; }
        public string Reason { get; set; }
    }
}
