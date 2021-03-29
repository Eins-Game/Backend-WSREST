using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Lobby
{
    public class Lobby
    {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public GeneralSettings GeneralSettings { get; set; }
        public Dictionary<int, IBasePlayer> Players { get; set; } = new Dictionary<int, IBasePlayer>();
    }
}
