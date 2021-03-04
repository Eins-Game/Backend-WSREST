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
