using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyPlayerLeftEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public StrippedEntities.LobbyPlayer Player { get; set; }
    }
}
