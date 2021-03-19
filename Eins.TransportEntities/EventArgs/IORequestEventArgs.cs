using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class IORequestEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public string RequestedType { get; set; }
    }
}
