﻿using Eins.TransportEntities;
using Eins.TransportEntities.Eins;
using Eins.TransportEntities.EventArgs;
using Eins.TransportEntities.EventArgs.StrippedEntities;
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
    public class LobbyHub : Hub
    {
        private readonly ILogger<LobbyHub> logger;
        private readonly ConcurrentDictionary<ulong, Lobby> lobbies;
        private readonly ConcurrentDictionary<ulong, EinsGame> games;
        private readonly ConcurrentDictionary<ulong, SessionUser> players;

        public LobbyHub(ILogger<LobbyHub> logger,
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, EinsGame> games,
            ConcurrentDictionary<ulong, SessionUser> players)
        {
            this.logger = logger;
            this.lobbies = lobbies;
            this.games = games;
            this.players = players;
        }

        public async Task Authenticate(string userName = default)
        {
            if (this.players.Any(x => x.Value.LobbyConnectionId == this.Context.ConnectionId))
                return;

            var sessuser = new SessionUser
            {
                LobbyConnectionId = this.Context.ConnectionId,
                Secret = Guid.NewGuid(),
                UserId = Convert.ToUInt64(this.players.Count),
                UserName = userName
            };
            var added = this.players.TryAdd(Convert.ToUInt64(this.players.Count), sessuser);
            await this.Clients.Caller.SendAsync("Authenticated", 200, new AuthenticatedEventArgs
            {
                Code = 200,
                UserSession = sessuser
            });
        }

        public async Task ReAuthenticate(Guid secret, string newUserName = default)
        {
            var user = this.players.FirstOrDefault(x => x.Value.Secret == secret);
            if (user.Value == default)
                return;
            if (newUserName != default)
                user.Value.UserName = newUserName;
            user.Value.LobbyConnectionId = this.Context.ConnectionId;

            await this.Clients.Caller.SendAsync("Authenticated", 200, new AuthenticatedEventArgs
            {
                Code = 200,
                UserSession = user.Value
            });
        }

        public async Task GetAllLobbies()
        {
            await Task.Delay(0);
            var lobiesAsList = lobbies.ToList();
            await this.Clients.Caller.SendAsync("AllLobbies", 200, lobiesAsList);
        }

        //Falls spieler in in einer lobby -> Exception
        public async Task CreateLobby(string name, string password = default)
        {
            var firstPlayer = this.players.FirstOrDefault(x => x.Value.LobbyConnectionId == this.Context.ConnectionId);
            if (firstPlayer.Value == default)
            {
                return;
            }
            var newLobby = new Lobby()
            {
                GeneralSettings = new GeneralSettings
                {
                    MaxPlayers = 4,
                    Password = password
                },
                Name = name,
                Creator = firstPlayer.Value.LobbyConnectionId,
                GameRules = new EinsRules(),
                ID = Convert.ToUInt64(this.lobbies.Count)
            };
            var player = new EinsPlayer(firstPlayer.Value.UserId, firstPlayer.Value);
            newLobby.Players.Add(newLobby.Players.Count, player);
            this.lobbies.TryAdd(Convert.ToUInt64(this.lobbies.Count), newLobby);
            await this.Clients.Caller.SendAsync("LobbyCreated", 200, new LobbyCreatedEventArgs
            {
                Code = 200,
                Creator = newLobby.Creator,
                GameMode = newLobby.GameRules?.GetType().Name,
                LobbyId = newLobby.ID,
                MaxPlayers = newLobby.GeneralSettings.MaxPlayers,
                Name = newLobby.Name,
                Password = newLobby.GeneralSettings.Password,
                PlayerCount = newLobby.Players.Count
            });
            await this.Clients.Others.SendAsync("LobbyCreated", 200, new LobbyCreatedEventArgs
            {
                Code = 200,
                Creator = newLobby.Creator,
                GameMode = newLobby.GameRules?.GetType().Name,
                LobbyId = newLobby.ID,
                MaxPlayers = newLobby.GeneralSettings.MaxPlayers,
                Name = newLobby.Name,
                PlayerCount = newLobby.Players.Count
            });
        }

        public async Task RemoveLobby(ulong id)
        {
            if (!this.lobbies.ContainsKey(id))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[id];
            if (this.Context.ConnectionId != lobby.Creator)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(401, "Not the lobby creator"));
                return;
            }

            this.lobbies.Remove(id, out var removed);
            this.games.TryRemove(id, out _);
            await this.Clients.All.SendAsync("LobbyRemoved", 200, new LobbyRemovedEventArgs
            {
                Code = 200,
                LobbyId = removed.ID,
                LobbyName = removed.Name
            });
        }

        public async Task ChangeLobbyGeneralSettings(ulong id, GeneralSettings updatedSettings)
        {
            if (!this.lobbies.ContainsKey(id))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[id];
            if (this.Context.ConnectionId == lobby.Creator)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(401, "Not the lobby creator"));
                return;
            }

            lobby.GeneralSettings = updatedSettings;
            var lobbyPlayers = lobby.Players.Select(player => player.Value.UserSession.LobbyConnectionId);
            await this.Clients.AllExcept(lobbyPlayers).SendAsync("LobbyGeneralSettingsUpdated", 200, new LobbyGeneralSettingsUpdatedEventArgs
            {
                Code = 200,
                NewMaxPlayerCount = lobby.GeneralSettings.MaxPlayers
            });
            await this.Clients.Clients(lobbyPlayers).SendAsync("LobbyGeneralSettingsUpdated", 200, new LobbyGeneralSettingsUpdatedEventArgs
            {
                Code = 200,
                NewMaxPlayerCount = lobby.GeneralSettings.MaxPlayers,
                NewPassword = lobby.GeneralSettings.Password
            });
        }

        public async Task ChangeGameModeSettings(ulong id, EinsRules rules)
        {
            if (!this.lobbies.ContainsKey(id))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[id];
            if (this.Context.ConnectionId != lobby.Creator)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(401, "Not the lobby creator"));
                return;
            }

            lobby.GameRules = rules;
            var lobbyPlayers = lobby.Players.Select(player => player.Value.UserSession.LobbyConnectionId);
            await this.Clients.Clients(lobbyPlayers).SendAsync("LobbyGameModeSettingsUpdated", 200, new LobbyGameModeSettingsUpdatedEventArgs
            {
                Code = 100,
                GameRules = rules
            });
        }

        public async Task PlayerJoin(ulong lobbyID, string password)
        {
            if (!this.lobbies.ContainsKey(lobbyID))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[lobbyID];
            if (lobby.Password != password)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(401, "Invalid Password"));
                return;
            }
            var player = this.players.First(x => x.Value.LobbyConnectionId == this.Context.ConnectionId).Value;
            lobby.Players.Add(lobby.Players.Count, new EinsPlayer(player.UserId, player, player.UserName, false));
            await this.Clients.All.SendAsync("PlayerJoined", 200, new LobbyPlayerJoinedEventArgs
            {
                Code = 200,
                Player = new LobbyPlayer
                {
                    ConnectionID = player.LobbyConnectionId,
                    ID = player.UserId,
                    Username = player.UserName,
                    IsBot = false
                }
            });
        }

        public async Task PlayerLeft(ulong lobbyID)
        {
            if (!this.lobbies.ContainsKey(lobbyID))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[lobbyID];
            if (!lobby.Players.Any(x => x.Value.UserSession.LobbyConnectionId == this.Context.ConnectionId))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(400, "Not in that lobby"));
                return;
            }
            var player = this.players.First(x => x.Value.LobbyConnectionId == this.Context.ConnectionId).Value;
            if (player.LobbyConnectionId != this.Context.ConnectionId)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(403, "Cant make other leave"));
                return;
            }
            var remove = lobby.Players.First(x => x.Value.UserSession.LobbyConnectionId == player.LobbyConnectionId);
            lobby.Players.Remove(remove.Key);
            await this.Clients.All.SendAsync("PlayerLeft", 200, new LobbyPlayerLeftEventArgs
            {
                Code = 200,
                Player = new LobbyPlayer
                {
                    ConnectionID = player.LobbyConnectionId,
                    ID = player.UserId,
                    Username = player.UserName,
                    IsBot = false
                }
            });
            if (lobby.Players.Count == 0)
                await this.RemoveLobby(lobby.ID);
            else
            {
                if (this.Context.ConnectionId == lobby.Creator)
                {
                    lobby.Creator = lobby.Players.First(x => !x.Value.IsBot).Value.UserSession.LobbyConnectionId;
                    await this.Clients.All.SendAsync("PlayerPromoted", 200, new LobbyPlayerPromotedEventArgs
                    {
                        Code = 200,
                        PromotedPlayer = new LobbyPlayer
                        {
                            ConnectionID = lobby.Players.First(x => !x.Value.IsBot).Value.UserSession.LobbyConnectionId,
                            ID = lobby.Players.First(x => !x.Value.IsBot).Value.ID,
                            Username = lobby.Players.First(x => !x.Value.IsBot).Value.Username,
                            IsBot = lobby.Players.First(x => !x.Value.IsBot).Value.IsBot
                        }
                    });
                }
            }
        }

        public async Task CreateGame(ulong lobbyID)
        {
            var callerID = this.Context.ConnectionId;
            if (!lobbies.Any(x => x.Value.Creator == callerID))
            {
                return; //Not lobby host
            }
            var lobby = lobbies.First(x => x.Value.Creator == callerID);
            var game = new EinsGame(lobbyID, lobby.Value.Players.Select(x => (EinsPlayer)x.Value).ToList(), lobby.Value.GameRules);
            lobby.Value.Game = game;
            this.games.TryAdd(lobby.Key, game);
            await lobby.Value.Game.InitializeGame(this);
            await this.Clients
                .Clients(lobby.Value.Players.Select(x => x.Value.UserSession.LobbyConnectionId).ToList())
                .SendAsync("LobbyGameCreated", 200, new LobbyGameCreatedEventArgs
                {
                    Code = 200,
                    Message = "Success lol"
                });

        }

        //Gegebenenfalls Methode zum kicken von Spielern

        //public async Task ChangeGameMode(ulong id, )
        //{
        //    if (!this.lobbies.ContainsKey(id))
        //    {
        //        await this.Clients.Caller.SendAsync("LobbyException", 404, "Invalid lobby ID");
        //        return;
        //    }
        //
        //    var lobby = lobbies[id];
        //    if (this.Context.ConnectionId == lobby.Creator)
        //    {
        //        await this.Clients.Caller.SendAsync("LobbyException", 401, "Not the lobby creator");
        //        return;
        //    }
        //
        //    lobby.GameRules = rules;
        //    var lobbyPlayers = lobby.Players.Select(player => player.Value.ConnectionID);
        //    await this.Clients.Clients(lobbyPlayers).SendAsync("LobbyGameModeSettingsUpdated", "max spieler anzahl mit PW");
        //}

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (!this.players.Any(x => this.Context.ConnectionId == x.Value.LobbyConnectionId))
                await base.OnDisconnectedAsync(exception);
            
            var pl = this.players.First(x => this.Context.ConnectionId == x.Value.LobbyConnectionId);
            if (exception != null )
                _ = Task.Run(() => WaitForReconnect(pl.Key));
            else
            {
                if (!this.lobbies.Any(x => x.Value.Players.Any(x => x.Value.ID == pl.Key)))
                {
                    this.players.Remove(pl.Key, out _);
                    await base.OnDisconnectedAsync(exception);
                    return;
                }

                var lobby = this.lobbies.First(x => x.Value.Players.Any(x => x.Value.ID == pl.Key));
                await PlayerLeft(lobby.Key);
                this.players.Remove(pl.Key, out _);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Heartbeat()
        {
            await Task.Delay(10 * 1000);
            await this.Clients.Caller.SendAsync("Ack");
        }

        public async Task WaitForReconnect(ulong playerID)
        {
            string lastConID = this.players[playerID].LobbyConnectionId;
            await Task.Delay(3 * 60 * 1000);
            if (this.players[playerID].LobbyConnectionId == lastConID)
            {
                if (!this.lobbies.Any(x => x.Value.Players.Any(x => x.Value.ID == playerID)))
                {
                    this.players.Remove(playerID, out _);
                    return;
                }

                var lobby = this.lobbies.First(x => x.Value.Players.Any(x => x.Value.ID == playerID));
                await PlayerLeft(lobby.Key);
                this.players.Remove(playerID, out _);
            }
        }
    }
}
