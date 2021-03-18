using Eins.TransportEntities.Interfaces;
using Microsoft.AspNetCore.SignalR;
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
        public string CurrentPlayer { get; set; } = default;
        public GameStatus Status { get; set; } = GameStatus.NotIntialized;
        public EinsRules Rules { get; set; }
        public bool Reversed { get; set; } = false;

        #region IO

        public bool AwaitingUserInput { get; set; } = false;
        public Card.CardColor UserInputColor { get; set; } = 0;

        #endregion

        private Random _rnd { get; set; } = new Random();
        #endregion

        public Game(ulong id, IList<Player> players, EinsRules rules)
        {
            this.GameID = id;

            for (int i = 0; i < players.Count; i++)
                this.Players.Add(i, players[i]);

            this.Rules = rules;
        }

        //TODO: implement Initialize
        public Task<bool> InitializeGame(Hub hub = default)
        {
            foreach (var player in this.Players)
            {
                for (int i = 0; i < this.Rules.CardsPerPlayer; i++)
                {
                    player.Value.HeldCards.Add(GetRandomCard());
                }
            }
            this.Status = GameStatus.Initialized;
            return Task.FromResult(true);
        }

        //TODO: Implement StartGame
        public Task<bool> StartGame(Hub hub = default)
        {
            this.CurrentPlayer = this.Players[0].ConnectionID;

            //TODO: Check card events on intial card
            ((Stack<IBaseCard>)this.CurrentStack).Push(GetRandomCard());
            this.Status = GameStatus.Started;
            return Task.FromResult(true);
        }

        public Task<bool> CanPlay(string playerConnectionID, Hub hub = default)
            => Task.FromResult(playerConnectionID == this.CurrentPlayer);

        public Task<IBaseCard> DrawCard(string playerConnectionID, Hub hub = default)
        {
            var card = GetRandomCard();
            var player = this.Players.First(x => x.Value.ConnectionID == playerConnectionID);
            player.Value.HeldCards.Add(card);
            return Task.FromResult(card);
        }

        //TODO: Implement Card Push
        public async Task<bool> PushCard(string playerConnectionID, IBaseCard card, Hub hub = default)
        {
            var player = this.Players.First(x => x.Value.ConnectionID == playerConnectionID);
            if (card is ActionCard actionCard)
            {
                switch (actionCard.CardType)
                {
                    case ActionCard.ActionCardType.Draw2:
                        await DoPlus2(hub);
                        break;
                    case ActionCard.ActionCardType.Draw4:
                        actionCard = await DoPlus4(hub, actionCard);

                        //Basically skip the next player, maybe do it with the SkipCard method later
                        var next = await GetNextPlayer();
                        this.CurrentPlayer = next.ConnectionID;
                        break;
                    case ActionCard.ActionCardType.Skip:
                        await DoSkip(hub);
                        break;
                    case ActionCard.ActionCardType.Switch:
                        await DoSwitch(hub);
                        break;
                    case ActionCard.ActionCardType.Wish:
                        actionCard = await DoWish(hub, actionCard);
                        break;
                    default:
                        break;
                }
                //Aussetzen -> CurrentPlayer + 2
                //+2 -> Spieler karten DrawCard 2 mal, CurrentPlayer + 2
                //+4 -> Spieler karten DrawCard 4 mal, CurrentPlayer + 2
                //WishCard -> LastStack card to wished Card color aka change wish card color
                //RichtungsWechsel -> ?
            }
            return false;
        }

        //TODO: Implement SetNextPlayer
        public Task<bool> SetNextPlayer(Hub hub = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGameFinished(Hub hub = default)
            => Task.FromResult(this.Players.Any(x => x.Value.HeldCards.Count == 0));

        private IBaseCard GetRandomCard()
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

        private Task<Player> GetNextPlayer()
        {

            var currentPlayer = this.Players.First(x => x.Value.ConnectionID == this.CurrentPlayer);
            if (this.Reversed)
            {
                var nextID = currentPlayer.Key - 1;
                if (nextID == -1)
                    nextID = this.Players.Count - 1;
                return Task.FromResult(this.Players[nextID] as Player);
            }
            else
            {
                var nextID = currentPlayer.Key + 1;
                if (nextID == this.Players.Count)
                    nextID = 0;
                return Task.FromResult(this.Players[nextID] as Player);
            }
        }

        private async Task<bool> DoPlus2(Hub hub)
        {
            var nextPlayer = await GetNextPlayer();
            var newCard1 = GetRandomCard();
            var newCard2 = GetRandomCard();
            nextPlayer.HeldCards.Add(newCard1);
            nextPlayer.HeldCards.Add(newCard2);
            var playerConnections = hub.Clients.Clients(Players.Select(x => x.Value.ConnectionID).ToArray());

            //Create CardDrawn EventArgs
            await playerConnections.SendAsync("CardDrawn", new object());
            return true;
        }

        private async Task<ActionCard> DoPlus4(Hub hub, ActionCard card)
        {
            var nextPlayer = await GetNextPlayer();
            var newCard1 = GetRandomCard();
            var newCard2 = GetRandomCard();
            var newCard3 = GetRandomCard();
            var newCard4 = GetRandomCard();
            nextPlayer.HeldCards.Add(newCard1);
            nextPlayer.HeldCards.Add(newCard2);
            nextPlayer.HeldCards.Add(newCard3);
            nextPlayer.HeldCards.Add(newCard4);
            var playerConnections = hub.Clients.Clients(Players.Select(x => x.Value.ConnectionID).ToArray());

            card.Color = this.UserInputColor;

            this.UserInputColor = 0;

            //Create ColorChange EventArgs
            await playerConnections.SendAsync("CardColorChanged", new object());


            //Create CardDrawn EventArgs
            await playerConnections.SendAsync("CardDrawn", new object());
            return card;
        }

        private async Task<ActionCard> DoWish(Hub hub, ActionCard card)
        {
            var playerConnections = hub.Clients.Clients(Players.Select(x => x.Value.ConnectionID).ToArray());

            card.Color = this.UserInputColor;

            this.UserInputColor = 0;

            await playerConnections.SendAsync("CardColorChanged", new object());
            return card;
        }
        private async Task<bool> DoSkip(Hub hub)
        {
            return true;
        }
        private async Task<bool> DoSwitch(Hub hub)
        {
            return true;
        }
    }
}
