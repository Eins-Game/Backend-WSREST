using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.GameSession
{
    public class PlayState
    {
        public bool ActivePlayer { get; set; } = false;

        //TODO: Replace with card object
        public List<string> HeldCards { get; set; } = new List<string>();
    }
}
