using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs.StrippedEntities
{
    public class Player
    {
        public ulong ID { get; set; }
        public int OrderID { get; set; }
        public string Username { get; set; }
        public string ConnectionID { get; set; }
        public int HeldCardAmount { get; set; }
    }
}
