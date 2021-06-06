using Eins.TransportEntities.Eins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eins.TransportEntities.Converters
{
    public class EinsActionCardConverter : JsonConverter<List<EinsActionCard>>
    {
        public override List<EinsActionCard> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            return null;
        }

        public override void Write(Utf8JsonWriter writer, List<EinsActionCard> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
