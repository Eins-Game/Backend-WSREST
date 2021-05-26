using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyGameCreatedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
