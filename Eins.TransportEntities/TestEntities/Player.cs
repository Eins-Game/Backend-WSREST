using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.TestEntities
{
    public class Player
    {
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public List<Card> HeldCards { get; set; } = new List<Card>();
    }
}
