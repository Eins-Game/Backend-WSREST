using Eins.TransportEntities;
using Eins.TransportEntities.EventArgs;
using Eins.TransportEntities.GameSession;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Hubs
{
    public class GameLobbyHub : Hub
    {
        private readonly ILogger<GameLobbyHub> _logger;
        private readonly ConcurrentDictionary<ulong, Lobby> _lobbies;
        private readonly ConcurrentDictionary<ulong, Game> _games;

        public GameLobbyHub(ILogger<GameLobbyHub> logger,
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, Game> games)
        {
            this._logger = logger;
            this._lobbies = lobbies;
            this._games = games;
        }

        /// <summary>
        /// Returns all lobbies, additionally only ones with specific attributes
        /// </summary>
        public async Task RequestLobbies(string options = default)
        {
            await this.Clients.Caller.SendAsync("LobbiesRequested", new LobbiesRequestedEventArgs
            {
                Lobbies = _lobbies.Values.ToList(),
                LobbyOptions = default
            });
        }

        /// <summary>
        /// Raised by client when trying to join a a lobby
        /// </summary>
        public async Task JoinGameLobby(Player player,ulong lobbyID, string password = "")
        {
            if (!_lobbies.TryGetValue(lobbyID, out var lobby))
                await this.Clients.Caller.SendAsync("JoinGameException", new JoinGameLobbyExceptionEventArgs
                {
                    ConnectionID = this.Context.ConnectionId,
                    Reason = "Lobby not found"
                });
            else if (lobby.GameInProgress)
                await this.Clients.Caller.SendAsync("JoinGameException", new JoinGameLobbyExceptionEventArgs
                {
                    ConnectionID = this.Context.ConnectionId,
                    Reason = "Game in progress"
                });
            else if (!string.IsNullOrWhiteSpace(lobby.Password) && lobby.Password != password)
                await this.Clients.Caller.SendAsync("JoinGameException", new JoinGameLobbyExceptionEventArgs
                {
                    ConnectionID = this.Context.ConnectionId,
                    Reason = "Wrong password"
                });
            else
            {
                player.ConnectionID = this.Context.ConnectionId;
                lobby.Players.TryAdd(player.ConnectionID, player);
                var players = lobby.Players.Select(x => x.Value.ConnectionID);
                await this.Clients.Clients(players).SendAsync("UserJoined", new UserJoinedEventAgs
                {
                    Lobby = lobby,
                    Player = player
                });
            }
        }

        /// <summary>
        /// Raised by client when thy are about to leave a lobby
        /// </summary>
        public async Task LeaveGameLobby(string playerConnectionID, ulong lobbyID)
        {
            if (!_lobbies.TryGetValue(lobbyID, out var lobby))
                await this.Clients.Caller.SendAsync("LeaveGameException", new JoinGameLobbyExceptionEventArgs
                {
                    ConnectionID = this.Context.ConnectionId,
                    Reason = "Lobby not found"
                });
            else
            {
                lobby.Players.Remove(playerConnectionID, out var player);
                if (lobby.LobbyCreator.PlayerID == player.PlayerID)
                {
                    if (lobby.Players.Count == 1)
                    {
                        _lobbies.Remove(lobbyID, out var removedlobby);
                        await this.Clients.All.SendAsync("LobbyRemoved", new LobbyRemovedEventArgs
                        {
                            Lobby = removedlobby
                        });
                    }
                    else
                    {
                        var newCreator = lobby.Players.First().Value;
                        lobby.LobbyCreator = newCreator;
                        await this.Clients.All.SendAsync("LobbyCreatorUserChanged", new LobbyCreatorUserChangedEventArgs
                        {
                            Lobby = lobby,
                            NewCreator = newCreator,
                            OldCreator = player
                        });
                    }
                }
                var rescipients = new List<string>(lobby.Players.Values.Select(x => x.ConnectionID));
                rescipients.Add(playerConnectionID);
                await this.Clients.Clients(rescipients).SendAsync("UserLeft", new UserLeftEventArgs
                {
                    Lobby = lobby,
                    Player = player,
                    Reason = "Left game/lobby"
                });
            }
        }


    }
}
