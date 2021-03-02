using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyGeneralSettingsChangedEventArgs
    {
        public Lobby Before { get; set; }
        public Lobby After { get; set; }
    }
}
