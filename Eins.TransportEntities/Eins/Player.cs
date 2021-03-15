using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Eins
{
    public class Player : IBasePlayer
    {
        public ulong ID { get; set; }
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public bool IsBot { get; set; } = false;
        public List<IBaseCard> HeldCards { get; set; } = new List<IBaseCard>();

        public Player(ulong id, string connectionID, string userName = default, bool isBot = false)
        {
            this.ID = id;
            this.ConnectionID = connectionID;

            //TODO: Do random if default
            if (userName == default)
                this.Username = userName;
            else
                this.Username = $"unnamed{id}";

            this.IsBot = isBot;
        }
    }
}
