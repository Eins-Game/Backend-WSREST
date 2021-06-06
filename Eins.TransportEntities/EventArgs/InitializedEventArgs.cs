using Eins.TransportEntities.Converters;
using Eins.TransportEntities.Eins;
using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Eins.TransportEntities.EventArgs
{
    public class InitializedEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }

        public List<StrippedEntities.Player> Players { get; set; }
        [JsonConverter(typeof(EinsCardListConverter))]
        public List<IBaseCard> YourCards { get; set; }
    }
}
