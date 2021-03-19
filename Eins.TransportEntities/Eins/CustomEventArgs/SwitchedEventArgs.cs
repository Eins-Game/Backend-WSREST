using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Eins.CustomEventArgs
{
    class SwitchedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public string NextPlayerConnectionID { get; set; }
        public string NextPlayerUsername { get; set; }
        public ulong NextPlayerID { get; set; }
    }
}
