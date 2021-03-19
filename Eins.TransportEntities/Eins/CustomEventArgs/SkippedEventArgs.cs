using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Eins.CustomEventArgs
{
    public class SkippedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public ulong SkippedPlayerID { get; set; }
        public string SkippedPlayerUsername { get; set; }
        public string SkippedPlayerConnectionID { get; set; }
    }
}
