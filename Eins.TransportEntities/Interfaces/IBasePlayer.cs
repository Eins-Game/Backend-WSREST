using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Interfaces
{
    public interface IBasePlayer
    {
        public ulong ID { get; set; }
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public bool IsBot { get; set; }
        public List<IBaseCard> HeldCards { get; set; }
    }
}
