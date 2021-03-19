using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class InitializedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }

        public List<StrippedEntities.Player> Players { get; set; }
        public List<IBaseCard> YourCards { get; set; }
    }
}
