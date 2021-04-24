using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities
{
    public class SessionUser
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public string LobbyConnectionId { get; set; }
        public string GameConnectionId { get; set; }
        public Guid Secret { get; set; }
    }
}
