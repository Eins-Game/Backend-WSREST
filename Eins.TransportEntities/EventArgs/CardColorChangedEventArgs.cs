using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Eins.TransportEntities.Eins.EinsCard;

namespace Eins.TransportEntities.EventArgs
{
    public class CardColorChangedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public CardColor OldColor { get; set; }
        public CardColor NewColor { get; set; }
    }
}
