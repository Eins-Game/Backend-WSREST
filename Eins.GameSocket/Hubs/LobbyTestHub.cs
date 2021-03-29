using Eins.TransportEntities.Eins;
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
    public class LobbyTestHub : Hub
    {
        private readonly ILogger<LobbyTestHub> logger;
        private readonly ConcurrentDictionary<ulong, Lobby> lobbies;
        private readonly ConcurrentDictionary<ulong, Game> games;

        public LobbyTestHub(ILogger<LobbyTestHub> logger,
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, Game> games)
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

        public async Task CreateLobby(string name, string password = default)
        {
            var newLobby = new Lobby()
            {
                GeneralSettings = new GeneralSettings(4, password),
                Name = name
            };
            newLobby.Players.Add(newLobby.Players.Count, new Player(0, this.Context.ConnectionId));
            this.lobbies.TryAdd(Convert.ToUInt64(this.lobbies.Count), newLobby);
            await this.Clients.Caller.SendAsync("LobbyCreated", "Lobby stuff mit PW");
            await this.Clients.Others.SendAsync("LobbyCreated", "Lobby stuff ohne PW");
        }
    }
}
