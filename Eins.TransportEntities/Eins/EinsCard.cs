using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class EinsCard : IBaseCard
    {
        public int Value { get; set; }
        public CardColor Color { get; set; }

        public EinsCard(int value, CardColor color)
        {
            this.Value = value;
            this.Color = color;
        }
        public EinsCard()
        {}

        public Task<bool> IsPlayable(IBaseCard card2)
        {
            // card2 = oberste Stapel Karte
            var einsCard = (EinsCard)card2;
            if ((einsCard.Color.HasFlag(this.Color) ||
                (this.Value == einsCard.Value)))
            {
                return Task.FromResult(true);
            }
            else return Task.FromResult(false);
        }

        public enum CardColor
        {
            Yellow = 1,
            Red = 2,
            Blue = 4,
            Green = 8,
            Black = 15
        }
    }
}
