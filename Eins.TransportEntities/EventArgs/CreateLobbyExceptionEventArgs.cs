using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class CreateLobbyExceptionEventArgs
    {
        public Player Player { get; set; }
        public string Reason { get; set; }
    }
}
