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
    public class LobbyHub : Hub
    {
        private readonly ILogger<LobbyHub> logger;
        private readonly ConcurrentDictionary<ulong, Lobby> lobbies;
        private readonly ConcurrentDictionary<ulong, EinsGame> games;

        public LobbyHub(ILogger<LobbyHub> logger,
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, EinsGame> games)
        {
            this.logger = logger;
            this.lobbies = lobbies;
            this.games = games;
        }

        public async Task GetAllLobbies()
        {
            await Task.Delay(0);
            var lobiesAsList = lobbies.ToList();

        }

        //Falls spieler in in einer lobby -> Exception
        //LobbyCreatedEvents erstellen und einfügen, jeweils mit und ohne Passwort
        public async Task CreateLobby(string name, string password = default)
        {
            var newLobby = new Lobby()
            {
                GeneralSettings = new GeneralSettings
                {
                    MaxPlayers = 4,
                    Password = password
                },
                Name = name,
                Creator = this.Context.ConnectionId,
                GameRules = new EinsRules()
            };
            var firstPlayer = new EinsPlayer(0, this.Context.ConnectionId);
            newLobby.Players.Add(newLobby.Players.Count, firstPlayer);
            this.lobbies.TryAdd(Convert.ToUInt64(this.lobbies.Count), newLobby);
            await this.Clients.Caller.SendAsync("LobbyCreated", "Lobby stuff mit PW");
            await this.Clients.Others.SendAsync("LobbyCreated", "Lobby stuff ohne PW");
        }

        //EventArgs objekt für "LobbyRemoved" erstellen und einfügen
        public async Task RemoveLobby(ulong id)
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

            this.lobbies.Remove(id, out _);
            await this.Clients.All.SendAsync("LobbyRemoved", "ID der lobby");
        }

        //EventArgs erstellen "LobbyGeneralSettingsUpdated" erstellen, 2 mal, jeweils mit und ohne Passwort
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
            var lobbyPlayers = lobby.Players.Select(player => player.Value.ConnectionID);
            await this.Clients.AllExcept(lobbyPlayers).SendAsync("LobbyGeneralSettingsUpdated", "max spieler anzahl ohne PW");
            await this.Clients.Clients(lobbyPlayers).SendAsync("LobbyGeneralSettingsUpdated", "max spieler anzahl mit PW");
        }

        //Eventargs erstellen für "LobbyGameModeSettingsUpdated" 
        public async Task ChangeGameModeSettings(ulong id, EinsRules rules)
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

            lobby.GameRules = rules;
            var lobbyPlayers = lobby.Players.Select(player => player.Value.ConnectionID);
            await this.Clients.Clients(lobbyPlayers).SendAsync("LobbyGameModeSettingsUpdated", "game pode settings properties");
        }

        //EventArgs für "PlayerJoined", am besten mit den meisten infos aus dem "player" Parameter
        public async Task PlayerJoin(ulong lobbyID, IBasePlayer player, string password)
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
            lobby.Players.Add(lobby.Players.Count, player);
            await this.Clients.All.SendAsync("PlayerJoined", "player info bzw. IBasePlayer");
        }

        //EventArgs für "PlayerLeft", am besten mit den meisten infos aus dem "player" Parameter
        public async Task PlayerLeft(ulong lobbyID, IBasePlayer player)
        {
            if (!this.lobbies.ContainsKey(lobbyID))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(404, "Invalid lobby ID"));
                return;
            }

            var lobby = lobbies[lobbyID];
            if (!lobby.Players.Any(x => x.Value.ConnectionID == this.Context.ConnectionId))
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(400, "Not in that lobby"));
                return;
            }
            if (player.ConnectionID != this.Context.ConnectionId)
            {
                await this.Clients.Caller.SendAsync("LobbyException", new ExceptionEventArgs(403, "Cant make other leave"));
                return;
            }
            var remove = lobby.Players.First(x => x.Value.ConnectionID == player.ConnectionID);
            lobby.Players.Remove(remove.Key);
            await this.Clients.All.SendAsync("PlayerLeft", "player info bzw. IBasePlayer");
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

    }
}
