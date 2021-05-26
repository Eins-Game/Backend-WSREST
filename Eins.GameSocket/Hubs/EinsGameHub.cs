using Eins.TransportEntities;
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
        private readonly ConcurrentDictionary<ulong, EinsGame> _games;
        private readonly ConcurrentDictionary<ulong, SessionUser> _players;
        Random _r = new Random();

        public EinsGameHub(ILogger<EinsGameHub> logger, 
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, EinsGame> games,
            ConcurrentDictionary<ulong, SessionUser> players)
        {
            this._logger = logger;
            this._lobbies = lobbies;
            this._games = games;
            this._players = players;
        }

        public async Task Authenticate(string lobbyConnectionID)
        {
            if (this._players.Any(x => x.Value.LobbyConnectionId != lobbyConnectionID))
                return;

            var player = this._players.First(x => x.Value.LobbyConnectionId == lobbyConnectionID);
            player.Value.GameConnectionId = this.Context.ConnectionId;

            await this.Clients.Caller.SendAsync("GamHubAuthenticated", 200, new AuthenticatedEventArgs
            {
                Code = 200,
                UserSession = player.Value
            });
        }

        public async Task ReAuthenticate(Guid secret)
        {
            var user = this._players.FirstOrDefault(x => x.Value.Secret == secret);
            if (user.Value == default)
                return;

            user.Value.GameConnectionId = this.Context.ConnectionId;

            await this.Clients.Caller.SendAsync("Authenticated", 200, new AuthenticatedEventArgs
            {
                Code = 200,
                UserSession = user.Value
            });
        }

        public async Task JoinGame(ulong lobbyID)
        {
            if (this._lobbies[lobbyID].Game == default)
                return; //No Game started lol
            var game = this._lobbies[lobbyID];
            var pl = game.Game.Players.First(x => x.Value.UserSession.GameConnectionId == this.Context.ConnectionId);
            pl.Value.IsReady = true;
            await this.Clients.Caller.SendAsync("GameJoined", 200, new GameJoinedEventArgs
            {
                Code = 200,
                Player = pl.Value
            });
            if (game.Game.Players.All(x => x.Value.IsReady))
            {
                await game.Game.StartGame(this);
            }
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

            var player = game.Players.First(x => x.Value.UserSession.GameConnectionId == this.Context.ConnectionId);
            var einsPlayer = player.Value as EinsPlayer;

            var hasCard = await einsPlayer.HasCardAsync((EinsCard)card);
            if (!hasCard)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(400, "You dont have a card like that"));
                return;
            }
            var playerConnections = this.Clients.Clients(game.Players.Select(x => x.Value.UserSession.GameConnectionId));

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
                    ConnectionID = player.Value.UserSession.GameConnectionId,
                    ID = player.Value.ID,
                    OrderID = player.Key,
                    Username = player.Value.Username
                }
            };

            await playerConnections.SendAsync("CardPlayed", playedArgs);

            if (card is EinsActionCard actionCard)
            {
                if (actionCard.Color == EinsCard.CardColor.Black)
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

        public async Task DoInteraction(ulong gameID, EinsCard.CardColor color)
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
                OldColor = EinsCard.CardColor.Black
            };

            var playerConnections = this.Clients.Clients(game.Players.Select(x => x.Value.UserSession.GameConnectionId));
            await playerConnections.SendAsync("CardColorChanged", colorChangeArgs);

            game.AwaitingUserInput = false;
        }

        public async Task DrawCard(ulong gameID)
        {
            var game = this._games[gameID];
            var player = game.Players.First(x => x.Value.UserSession.GameConnectionId == this.Context.ConnectionId);

            var canDraw = await game.CanPlayAsync(this.Context.ConnectionId);
            if (!canDraw)
            {
                await this.Clients.Caller.SendAsync("GameException", new ExceptionEventArgs(401, "Not your turn"));
                return;
            }

            var card = await game.DrawCard(player.Value.UserSession.GameConnectionId);

            var drawnArgsOthers = new CardDrawnEventArgs(200, player.Value, player.Key);
            var drawnArgsThem = new CardDrawnEventArgs(200, player.Value, player.Key, card);

            var playerConnectionsOthers = this.Clients.Clients(game.Players
                .Where(x => x.Value.UserSession.GameConnectionId != this.Context.ConnectionId)
                .Select(x => x.Value.UserSession.GameConnectionId));

            await playerConnectionsOthers.SendAsync("CardDrawn", drawnArgsOthers);
            await this.Clients.Caller.SendAsync("CardDrawn", drawnArgsThem);

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (!this._players.Any(x => this.Context.ConnectionId == x.Value.LobbyConnectionId))
                return base.OnDisconnectedAsync(exception);

            var pl = this._players.First(x => this.Context.ConnectionId == x.Value.LobbyConnectionId);
            _ = Task.Run(() => WaitForReconnect(pl.Key));
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Heartbeat()
        {
            await Task.Delay(10 * 1000);
            await this.Clients.Caller.SendAsync("Ack");
        }

        public async Task WaitForReconnect(ulong playerID)
        {
            string lastConID = this._players[playerID].GameConnectionId;
            await Task.Delay(3 * 60 * 1000);
            if (this._players[playerID].GameConnectionId == lastConID)
            {
                this._players.Remove(playerID, out _);
                if (!this._games.Any(x => x.Value.Players.Any(x => x.Value.ID == playerID)))
                    return;
                var game = this._games.First(x => x.Value.Players.Any(x => x.Value.ID == playerID));
                if (game.Value.CurrentPlayer == lastConID)
                {
                    await game.Value.SetNextPlayer(this);
                }
                var gamePl = game.Value.Players.First(x => x.Value.ID == playerID);
                game.Value.Players.Remove(gamePl.Key);
            }
        }
    }
}
