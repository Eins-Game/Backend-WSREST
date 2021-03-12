using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.Lobby
{
    public class GeneralSettings
    {
        public string Password { get; set; }
        public int MaxPlayers { get; set; }
        public GeneralSettings(int maxPlayers, string password = default)
        {
            this.Password = password;
            this.MaxPlayers = maxPlayers;
        }
    }
}
