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

        //Position (zeroIndex)
        public Dictionary<int, IBasePlayer> Players { get; set; }

        //ConnectionID
        public string CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }

        public Task<bool> InitializeGame();
        public Task<bool> StartGame();
        public Task<bool> CanPlay(string playerConnectionID);
        public Task<bool> SetNextPlayer();
        public Task<bool> PushCard(string playerConnectionID, IBaseCard card);
        public Task<bool> IsGameFinished();
    }

    public enum GameStatus
    {
        NotIntialized,
        Initialized,
        Started,
        Ended
    }
}
