using Eins.TransportEntities.TestEntities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Hubs
{
    public class EinsHub : Hub
    {
        private readonly ILogger<EinsHub> _logger;

        //SingleGame -> Temporary until Lobby ligic
        private readonly Game _game;
        Random _r = new Random();

        public EinsHub(ILogger<EinsHub> logger, Game game)
        {
            this._logger = logger;
            this._game = game;
        }

        public async Task JoinGame(string username = "unnamed")
        {
            var np = new Player
            {
                ConnectionID = this.Context.ConnectionId,
                Username = username
            };
            for (int i = 0; i < 7; i++)
                np.HeldCards.Add(GetRandomCard());
            this._game.Players.Add(this._game.Players.Count, np);
            if (this._game.CurrentPlayer == default)
            {
                this._game.CurrentPlayer = this.Context.ConnectionId;
                this._game.CurrentStack.Push(GetRandomCard());
                await this.Clients.Client(this._game.CurrentPlayer).SendAsync("TurnNotification", 103, this._game.CurrentStack.Peek(), np);
            }

            await this.Clients.All.SendAsync("JoinGameSuccess", 200, $"\"{username}\" Sucessfully connected (Player {_game.Players.Count-1})");
        }

        //Validate playbility on client, but also validated here just in case
        public async Task PlayCard(Card card)
        {
            if (this.Context.ConnectionId != this._game.CurrentPlayer)
            {
                await this.Clients.Caller
                    .SendAsync("PlayCardFailed", 401, "Not your turn");
                return;
            }
            var curPlayer = this._game.Players.First(x => x.Value.ConnectionID == this.Context.ConnectionId);
            if (!curPlayer.Value.HeldCards.Any(x => x == card)){
                await this.Clients.Caller
                    .SendAsync("PlayCardFailed", 401, "You dont have a card like that");
                return;
            }

            var topCard = this._game.CurrentStack.Peek();

            if (card.Color.HasFlag(topCard.Color) || card.Value == topCard.Value)
            {
                this._game.CurrentStack.Push(card);
                curPlayer.Value.HeldCards.Remove(card);
                var curPlayerID = curPlayer.Key;
                curPlayerID++;
                if (curPlayerID == this._game.Players.Count)
                    curPlayerID = 0;
                this._game.CurrentPlayer = this._game.Players[curPlayerID].ConnectionID;
                await this.Clients.All.SendAsync("PlayCardSuccess", 
                    200, 
                    curPlayer.Value, 
                    card);
                await this.Clients.Client(this._game.CurrentPlayer)
                    .SendAsync("TurnNotification",
                    103, 
                    card, 
                    this._game.Players[curPlayerID]);
            }
            else
                await this.Clients.Caller
                    .SendAsync("PlayCardFailed", 400, "Card not playable");
        }

        public async Task DrawCard()
        {
            if (this.Context.ConnectionId != this._game.CurrentPlayer)
            {
                await this.Clients.Caller
                    .SendAsync("DrawCardFailed", 401, "Not your turn");
                return;
            }
            var player = this._game.Players
                .First(x => x.Value.ConnectionID == this.Context.ConnectionId);
            var rndCard = GetRandomCard();
            this._game.Players[player.Key].HeldCards.Add(rndCard);
            await this.Clients.Caller.SendAsync("DrawCardSuccess", 200, rndCard);
        }

        private Card GetRandomCard()
        {
            var vals = Enum.GetValues<CardColor>();
            var card = new Card
            {
                Color = vals[this._r.Next(0, 4)],
                Value = this._r.Next(0, 10)
            };
            return card;
        }
    }
}
