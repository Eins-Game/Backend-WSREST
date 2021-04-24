using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs.StrippedEntities
{
    public class LobbyPlayer
    {
        public ulong ID { get; set; }
        public string Username { get; set; }
        public string ConnectionID { get; set; }
        public bool IsBot { get; set; }
    }
}
