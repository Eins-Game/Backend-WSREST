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

        public EinsHub(ILogger<EinsHub> logger, Game game)
        {
            this._logger = logger;
            this._game = game;
        }

        public async Task JoinGame()
        {
            this._game.Players.Add(this._game.Players.Count, this.Context.ConnectionId);
            if (this._game.CurrentPlayer == default)
            {
                this._game.CurrentPlayer = this.Context.ConnectionId;
                this._game.CurrentStack.Push(new Card
                {
                    Color = CardColor.Red,
                    Value = 3
                });
                await this.Clients.Client(this._game.CurrentPlayer).SendAsync("TurnNotification", 103, this._game.CurrentStack.Peek());
            }

            await this.Clients.All.SendAsync("JoinGameSuccess", 200, $"\"{this.Context.ConnectionId}\" Sucessfully connected (Player {_game.Players.Count-1})");
        }

        //Validate playbility on client, but also validated here just in case
        public async Task PlayCard(Card card)
        {
            if (this.Context.ConnectionId != this._game.CurrentPlayer)
            {
                await this.Clients.Caller.SendAsync("PlayCardFailed", 401, "Not your turn");
                return;
            }

            var topCard = this._game.CurrentStack.Peek();

            if (card.Color.HasFlag(topCard.Color) || card.Value == topCard.Value)
            {
                this._game.CurrentStack.Push(card);
                var curPlayerID = this._game.Players.First(x => x.Value == this.Context.ConnectionId).Key;
                curPlayerID++;
                if (curPlayerID == this._game.Players.Count)
                    curPlayerID = 0;
                this._game.CurrentPlayer = this._game.Players[curPlayerID];
                await this.Clients.All.SendAsync("PlayCardSuccess", 200, this.Context.ConnectionId, card);
                await this.Clients.Client(this._game.CurrentPlayer).SendAsync("TurnNotification", 103, card);
            }
            else
                await this.Clients.Caller.SendAsync("PlayCardFailed", 400, "Card not playable");
        }
    }
}
