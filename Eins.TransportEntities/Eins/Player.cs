using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class Player : IBasePlayer
    {
        public ulong ID { get; set; }
        public string ConnectionID { get; set; }
        public string Username { get; set; }
        public bool IsBot { get; set; } = false;
        public List<IBaseCard> HeldCards { get; set; } = new List<IBaseCard>();

        public Player(ulong id, string connectionID, string userName = default, bool isBot = false)
        {
            this.ID = id;
            this.ConnectionID = connectionID;

            if (userName == default)
                this.Username = userName;
            else
                this.Username = $"unnamed{id}";

            this.IsBot = isBot;
        }

        public Task<bool> HasCardAsync(Card card)
        {
            if (card is ActionCard actionCard)
            {
                var actCards = this.HeldCards.Where(x => x is ActionCard) as IEnumerable<ActionCard>;
                return Task.FromResult(actCards.Any(x => x.CardType == actionCard.CardType
                && x.Color == actionCard.Color));
            }
            return Task.FromResult(HeldCards.Any(x => ((Card)x).Color == card.Color
                && ((Card)x).Value == card.Value));
        }
    }
}
