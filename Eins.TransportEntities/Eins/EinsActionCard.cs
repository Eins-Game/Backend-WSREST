using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class EinsActionCard : EinsCard
    {
        public ActionCardType CardType { get; set; }
        public EinsActionCard(CardColor color, ActionCardType action) : base(-1,  color)
        {
            this.CardType = action;
        }
        public EinsActionCard()
        {}

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
