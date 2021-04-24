using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyRemovedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public ulong LobbyId { get; set; }
        public string LobbyName { get; set; }
    }
}
