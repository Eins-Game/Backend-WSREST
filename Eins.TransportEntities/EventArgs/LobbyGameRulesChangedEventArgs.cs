using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyGameRulesChangedEventArgs
    {
        public object Before { get; set; }
        public object After { get; set; }
    }
}
