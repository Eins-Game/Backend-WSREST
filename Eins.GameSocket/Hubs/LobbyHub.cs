using Eins.TransportEntities;
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
    public class LobbyHub : Hub
    {
        private readonly ILogger<LobbyHub> _logger;
        private readonly ConcurrentBag<Lobby> _lobbies;
        private readonly ConcurrentBag<Game> _games;
        private readonly ConcurrentDictionary<ulong, IClientProxy> _connectedUsers;

        public LobbyHub(ILogger<LobbyHub> logger, 
            ConcurrentBag<Lobby> lobbies, 
            ConcurrentBag<Game> games,
            ConcurrentDictionary<ulong, IClientProxy> connectedUsers)
        {
            this._logger = logger;
            this._lobbies = lobbies;
            this._games = games;
            this._connectedUsers = connectedUsers;
        }

        /// <summary>
        /// When a client connects, a "guest" session is created with a random ID and username (guest+ID)
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var val = _connectedUsers.GetOrAdd(Convert.ToUInt64(_connectedUsers.Count), this.Clients.Caller);
            var id = _connectedUsers.First(x => x.Value == val);
            await base.OnConnectedAsync();
            await val.SendAsync("InitialSessionEstablished", new Player
            {
                Username = $"guest{id.Key}",
                PlayerID = id.Key
            });
        }

        /// <summary>
        /// Removes the socketconnection for the dictionary, to ensure no zombie connections
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var id = _connectedUsers.First(x => x.Value == this.Clients.Caller);
            _connectedUsers.Remove(id.Key, out _);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CreateLobby(Player user, string lobbyName, string password = null)
        {
            return;
        }

        public async Task EnterLobby(Player user, ulong lobbySessionID, string password = null)
        {
            return;
        }

        public async Task LeaveLobby(Player user, Lobby lobby)
        {
            return;
        }
    }
}
