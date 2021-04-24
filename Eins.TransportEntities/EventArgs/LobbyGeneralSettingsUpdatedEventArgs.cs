using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyGeneralSettingsUpdatedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public int NewMaxPlayerCount { get; set; }
        public string NewPassword { get; set; }
    }
}
