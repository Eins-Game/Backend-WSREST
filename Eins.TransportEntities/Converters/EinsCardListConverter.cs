using Eins.TransportEntities.Eins;
using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Eins.TransportEntities.Converters
{
    public class EinsCardListConverter : JsonConverter<List<IBaseCard>>
    {
        public override List<IBaseCard> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            return null;

        }

        public override void Write(Utf8JsonWriter writer, List<IBaseCard> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                if (item is EinsCard e && !(item is EinsActionCard))
                {
                    writer.WriteStringValue(JsonSerializer.Serialize(new EinsActionCard
                    {
                        CardType = (EinsActionCard.ActionCardType)(-1),
                        Color = e.Color,
                        Value = e.Value
                    }));
                }
                else if (item is EinsActionCard a)
                {
                    writer.WriteStringValue(JsonSerializer.Serialize(a));
                }
            }
            writer.WriteEndArray();
        }
    }
}
