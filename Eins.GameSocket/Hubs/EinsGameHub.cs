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

        }

        //Validate playbility on client, but also validated here just in case
        public async Task PlayCard()
        {
            //If card await userinput
            //if card = black
            //-> game IO in progress = true
            //-> send IO request for color
            //continue with PlayCard
        }

        public async Task DoInteraction(Card.CardColor color)
        {
            //get color and set for game IO
            //set IO in progress = false;
        }

        public async Task DrawCard()
        {
        }
    }
}
