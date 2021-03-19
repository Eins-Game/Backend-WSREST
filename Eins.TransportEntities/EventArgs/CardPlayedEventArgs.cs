using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class CardPlayedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public IBaseCard Card { get; set; }
        public StrippedEntities.Player ByPlayer { get; set; }
    }
}
