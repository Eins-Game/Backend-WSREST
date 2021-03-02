using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class UserLeftEventArgs : UserJoinedEventAgs
    {
        public string Reason { get; set; }
    }
}
