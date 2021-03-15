using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Interfaces
{
    public interface IBaseGame
    {
        public ulong GameID { get; set; }
        public IEnumerable<IBaseCard> CurrentStack { get; set; }

        //TODO: Position (zeroIndex)
        public Dictionary<int, IBasePlayer> Players { get; set; }

        //TODO: ConnectionID
        public string CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }

        //TODO: Replace with game initialized event args
        public Task<bool> InitializeGame(HubConnection hub = default);

        //TODO: Replace with game started event args
        public Task<bool> StartGame(HubConnection hub = default);


        public Task<bool> CanPlay(string playerConnectionID, HubConnection hub = default);

        //TODO: Replace with next player event args
        public Task<bool> SetNextPlayer(HubConnection hub = default);

        //TODO: Replace with played card event args
        public Task<bool> PushCard(string playerConnectionID, IBaseCard card, HubConnection hub = default);

        //TODO: Replace with game ended event args
        public Task<bool> IsGameFinished(HubConnection hub = default);
        public Task<IBaseCard> DrawCard(string playerConnectionID, HubConnection hub = default);
    }

    public enum GameStatus
    {
        NotIntialized,
        Initialized,
        Started,
        Ended
    }
}
