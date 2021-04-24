using Eins.TransportEntities.Eins;
using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class LobbyGameModeSettingsUpdatedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }
        public EinsRules GameRules { get; set; }
    }
}
