using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.TestEntities
{
    public class Card
    {
        public int Value { get; set; }
        public CardColor Color { get; set; }

        //Value must be -1 for Event to be checked
        public CardEvent Event { get; set; }

        #region Overrides
        public override bool Equals(object obj)
        {
            if (obj is Card c)
                return (Value == c.Value)
                    && (Color == c.Color)
                    && (Event == c.Event);
            return base.Equals(obj);
        }
        public static bool operator ==(Card c1, Card c2) 
        {
            return (c1.Value == c2.Value)
                    && (c1.Color == c2.Color)
                    && (c1.Event == c2.Event);
        }
        public static bool operator !=(Card c1, Card c2)
        {
            return !((c1.Value == c2.Value)
                    && (c1.Color == c2.Color)
                    && (c1.Event == c2.Event));
        }
        public override int GetHashCode()
            => base.GetHashCode();
        #endregion
    }

    [Flags]
    public enum CardColor
    {
        Red = 1,
        Green = 2,
        Yellow = 4,
        Blue = 8,
        Black = 15
    }

    [Flags]
    public enum CardEvent
    {
        Draw2 = 1,
        Draw4 = 2,
        ChangeColor = 4,
        Invert = 8,
        Skip = 16
    }
}
