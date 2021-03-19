using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class GameStartedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public IBaseCard FirstCard { get; set; }
    }
}
