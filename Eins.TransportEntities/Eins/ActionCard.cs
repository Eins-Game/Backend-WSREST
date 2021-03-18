using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class ActionCard : Card
    {
        public ActionCardType CardType { get; set; }
        public ActionCard(CardColor color, ActionCardType action) : base(-1,  color)
        {
            this.CardType = action;
        }

        public enum ActionCardType
        {
            Draw2,
            Draw4,
            Skip,
            Switch,
            Wish
        }
    }
}
