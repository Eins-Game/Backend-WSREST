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
        #region Properties
        public ulong GameID { get; set; }
        public IEnumerable<IBaseCard> CurrentStack { get; set; } = new Stack<IBaseCard>();
        public Dictionary<int, IBasePlayer> Players { get; set; } = new Dictionary<int, IBasePlayer>();
        public string CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }
        public object Rules { get; set; }

        private Random _rnd { get; set; } = new Random();
        #endregion

        public Game(IList<Player> players, object rules)
        {
            for (int i = 0; i < players.Count; i++)
                this.Players.Add(i, players[i]);

            this.Rules = rules;
        }

        //TODO: implement Initialize
        public async Task<bool> InitializeGame(HubConnection hub = default)
        {
            foreach (var player in this.Players)
            {

            }
            this.Status = GameStatus.Initialized;
            return true;
        }

        //TODO: Implement StartGame
        public Task<bool> StartGame(HubConnection hub = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanPlay(string playerConnectionID, HubConnection hub = default)
            => Task.FromResult(playerConnectionID == this.CurrentPlayer);

        //TODO: Implement DrawCard
        public Task<IBaseCard> DrawCard(string playerConnectionID, HubConnection hub = default)
        {
            throw new NotImplementedException();
        }

        //TODO: Implement Card Push
        public async Task<bool> PushCard(string playerConnectionID, IBaseCard card, HubConnection hub = default)
        {
            var player = this.Players.First(x => x.Value.ConnectionID == playerConnectionID);
            if (card is ActionCard actionCard)
            {
                await actionCard.DoThing();
                //Aussetzen -> CurrentPlayer + 2
                //+2 -> Spieler karten DrawCard 2 mal, CurrentPlayer + 2
                //+4 -> Spieler karten DrawCard 4 mal, CurrentPlayer + 2
                //WishCard -> LastStack card to wished Card color aka change wish card color
                //RichtungsWechsel -> ?
            }
            return false;
        }

        //TODO: Implement SetNextPlayer
        public Task<bool> SetNextPlayer(HubConnection hub = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGameFinished(HubConnection hub = default)
            => Task.FromResult(this.Players.Any(x => x.Value.HeldCards.Count == 0));

        public Card GetRandomCard()
        {
            var colors = Enum.GetValues(typeof(Card.CardColor)) as Card.CardColor[];
            var color = colors[this._rnd.Next(0, 5)];

            if (color != Card.CardColor.Black)
                return new Card(this._rnd.Next(0, 10), color);

            else
            {
                var actions = Enum.GetValues(typeof(ActionCard.ActionCardType)) as ActionCard.ActionCardType[];
                return new ActionCard(color, actions[this._rnd.Next(0, 5)]);
            }
        }
    }
}
