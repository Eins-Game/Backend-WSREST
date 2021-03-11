using Eins.TransportEntities.Lobby;
using Eins.TransportEntities.TestEntities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eins.GameSocket.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly ConcurrentDictionary<ulong, Lobby> _lobbies;
        private readonly ConcurrentDictionary<ulong, Game> _games;
        Random _r = new Random();

        public GameHub(ILogger<GameHub> logger, 
            ConcurrentDictionary<ulong, Lobby> lobbies,
            ConcurrentDictionary<ulong, Game> games)
        {
            this._logger = logger;
            this._lobbies = lobbies;
            this._games = games;
        }

        public async Task JoinGame()
        {
        }

        //Validate playbility on client, but also validated here just in case
        public async Task PlayCard()
        {
        }

        public async Task DrawCard()
        {
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
