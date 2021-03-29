using Eins.TransportEntities.EventArgs;
using Microsoft.AspNetCore.SignalR;
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

        public object GameRules { get; set; }
        public Type RulesType { get; set; }

        //Zero index
        public Dictionary<int, IBasePlayer> Players { get; set; }

        //ConnectionID
        public string CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }

        public Task<bool> InitializeGame(Hub hub = default);

        public Task<GameStartedEventArgs> StartGame(Hub hub = default);


        public Task<bool> CanPlayAsync(string playerConnectionID, Hub hub = default);

        public Task<IBasePlayer> SetNextPlayer(Hub hub = default);

        public Task<bool> PushCard(string playerConnectionID, IBaseCard card, Hub hub = default);

        public Task<bool> IsGameFinished(Hub hub = default);
        public Task<IBaseCard> DrawCard(string playerConnectionID, Hub hub = default);
    }

    public enum GameStatus
    {
        NotIntialized,
        Initialized,
        Started,
        Ended
    }
}
