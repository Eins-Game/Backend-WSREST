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

            //TODO: check if true
            var canPlay = await game.CanPlay(this.Context.ConnectionId);

            var player = game.Players.First(x => x.Value.ConnectionID == this.Context.ConnectionId);
            var einsPlayer = player.Value as Player;

            //TODO: Check if true
            var hasCard = einsPlayer.HasCard((Card)card);
            var playerConnections = this.Clients.Clients(game.Players.Select(x => x.Value.ConnectionID));

            //TODO: Check if true
            var topCard = await game.GetTopCard();
            var validCard = await card.IsPlayable(topCard);

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

            //TODO: check if true
            var canPlay = await game.CanPlay(this.Context.ConnectionId);

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

            //TODO: check if true
            var canDraw = game.CanPlay(this.Context.ConnectionId);

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
            await Task.Delay(10 * 1000);
            await this.Clients.Caller.SendAsync("Heartbeat");
        }
    }
}
