using Eins.TransportEntities.Eins;
using Eins.TransportEntities.EventArgs;
using Eins.TransportEntities.Interfaces;
using Eins.TransportEntities.Lobby;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Hubs
{
    public class EinsGameHub : Hub
    {
        private readonly ILogger<EinsGameHub> _logger;
        private readonly ConcurrentDictionary<ulong, Lobby> _lobbies;
        private readonly ConcurrentDictionary<ulong, Game> _games;
        Random _r = new Random();

        public EinsGameHub(ILogger<EinsGameHub> logger, 
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, Game> games)
        {
            this._logger = logger;
            this._lobbies = lobbies;
            this._games = games;
        }

        public async Task JoinGame()
        {
            await Task.Delay(0);
            //When Lobby done
        }

        //Validate playbility on client, but also validated here just in case
        public async Task PlayCard(ulong gameID, IBaseCard card)
        {
            var game = this._games[gameID];

            var canPlay = await game.CanPlayAsync(this.Context.ConnectionId);
            if (!canPlay)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(401, "Not your turn"));
                return;
            }

            var player = game.Players.First(x => x.Value.ConnectionID == this.Context.ConnectionId);
            var einsPlayer = player.Value as Player;

            var hasCard = await einsPlayer.HasCardAsync((Card)card);
            if (!hasCard)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(400, "You dont have a card like that"));
                return;
            }
            var playerConnections = this.Clients.Clients(game.Players.Select(x => x.Value.ConnectionID));

            var topCard = await game.GetTopCardAsync();
            var validCard = await card.IsPlayable(topCard);
            if (!validCard)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(409, "Card is not playble"));
                return;
            }

            var playedArgs = new CardPlayedEventArgs
            {
                Code = 200,
                Card = card,
                ByPlayer = new TransportEntities.EventArgs.StrippedEntities.Player
                {
                    HeldCardAmount = player.Value.HeldCards.Count,
                    ConnectionID = player.Value.ConnectionID,
                    ID = player.Value.ID,
                    OrderID = player.Key,
                    Username = player.Value.Username
                }
            };

            await playerConnections.SendAsync("CardPlayed", playedArgs);

            if (card is ActionCard actionCard)
            {
                if (actionCard.Color == Card.CardColor.Black)
                {
                    game.AwaitingUserInput = true;

                    var ioArgs = new IORequestEventArgs
                    {
                        Code = 200,
                        RequestedType = "Card.Color"
                    };
                    await this.Clients.Caller.SendAsync("InteractionRequested", ioArgs);

                    while (game.AwaitingUserInput)
                    {
                        await Task.Delay(100);
                    }
                }
            }
            var wasPushed = game.PushCard(this.Context.ConnectionId, card, this);

            var nextPlayer = await game.SetNextPlayer();
        }

        public async Task DoInteraction(ulong gameID, Card.CardColor color)
        {
            var game = this._games[gameID];

            var canPlay = await game.CanPlayAsync(this.Context.ConnectionId);
            if (!canPlay)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(401, "Not your turn"));
                return;
            }

            game.UserInputColor = color;


            var colorChangeArgs = new CardColorChangedEventArgs
            {
                Code = 200,
                NewColor = color,
                OldColor = Card.CardColor.Black
            };

            var playerConnections = this.Clients.Clients(game.Players.Select(x => x.Value.ConnectionID));
            await playerConnections.SendAsync("CardColorChanged", colorChangeArgs);

            game.AwaitingUserInput = false;
        }

        public async Task DrawCard(ulong gameID)
        {
            var game = this._games[gameID];
            var player = game.Players.First(x => x.Value.ConnectionID == this.Context.ConnectionId);

            var canDraw = await game.CanPlayAsync(this.Context.ConnectionId);
            if (!canDraw)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(401, "Not your turn"));
                return;
            }

            var card = await game.DrawCard(player.Value.ConnectionID);

            var drawnArgsOthers = new CardDrawnEventArgs(200, player.Value, player.Key);
            var drawnArgsThem = new CardDrawnEventArgs(200, player.Value, player.Key, card);

            var playerConnectionsOthers = this.Clients.Clients(game.Players
                .Where(x => x.Value.ConnectionID != this.Context.ConnectionId)
                .Select(x => x.Value.ConnectionID));

            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers);
            await this.Clients.Caller.SendAsync("CardDrawn", drawnArgsThem);

        }

        public async Task HeartBeat()
        {
            //await Task.Delay(10 * 1000);
            await this.Clients.Caller.SendAsync("Heartbeat");
        }
    }
}
