using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class AuthenticatedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public SessionUser UserSession { get; set; }
    }
}
