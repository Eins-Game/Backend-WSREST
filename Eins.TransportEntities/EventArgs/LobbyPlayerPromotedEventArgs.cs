using Eins.TransportEntities.EventArgs.StrippedEntities;
using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyPlayerPromotedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public LobbyPlayer PromotedPlayer { get; set; }
    }
}
