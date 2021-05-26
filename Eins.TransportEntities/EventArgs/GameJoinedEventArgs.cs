using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class GameJoinedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public IBasePlayer Player { get; set; }
    }
}
