using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eins.TransportEntities.Eins
{
    public class Game : IBaseGame
    {
        public ulong GameID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IEnumerable<IBaseCard> CurrentStack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<int, IBasePlayer> Players { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CurrentPlayer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public GameStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task<bool> CanPlay(string playerConnectionID)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InitializeGame()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGameFinished()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PushCard(string playerConnectionID, IBaseCard card)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetNextPlayer()
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartGame()
        {
            throw new NotImplementedException();
        }
    }
}
