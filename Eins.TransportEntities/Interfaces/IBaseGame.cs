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
        public Task<bool> InitializeGame();

        //TODO: Replace with game started event args
        public Task<bool> StartGame();


        public Task<bool> CanPlay(string playerConnectionID);

        //TODO: Replace with next player event args
        public Task<bool> SetNextPlayer();

        //TODO: Replace with played card event args
        public Task<bool> PushCard(HubConnection hub, string playerConnectionID, IBaseCard card);

        //TODO: Replace with game ended event args
        public Task<bool> IsGameFinished();
        public Task<IBaseCard> DrawCard(string playerConnectionID);
    }

    public enum GameStatus
    {
        NotIntialized,
        Initialized,
        Started,
        Ended
    }
}
