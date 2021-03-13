using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class Game : IBaseGame
    {
        public ulong GameID { get; set; }
        public IEnumerable<IBaseCard> CurrentStack { get; set; } = new Stack<IBaseCard>();
        public Dictionary<int, IBasePlayer> Players { get; set; }
        public string CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }

        public Task<bool> CanPlay(string playerConnectionID)
            => Task.FromResult(playerConnectionID == this.CurrentPlayer);

        public Task<bool> InitializeGame()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGameFinished()
            => Task.FromResult(this.Players.Any(x => x.Value.HeldCards.Count == 0));

        //TODO: Implement Card Push
        public async Task<bool> PushCard(HubConnection hub, string playerConnectionID, IBaseCard card)
        {
            var player = this.Players.First(x => x.Value.ConnectionID == playerConnectionID);
            if (card is ActionCard actionCard)
            {
                await actionCard.DoThing();
                //Aussetzen -> CurrentPlayer + 2
                //+2 -> Spieler karten DrawCard 2 mal, CurrentPlayer + 2
                //+4 -> Spieler karten DrawCard 4 mal, CurrentPlayer + 2
                //WishCard -> LastStack card to w0shed Card
                //RichtungsWechsel -> ?
            }
            return false;
        }

        public Task<bool> SetNextPlayer()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartGame()
        {
            throw new NotImplementedException();
        }
    }
}
