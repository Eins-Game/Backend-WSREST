using Eins.TransportEntities.EventArgs.StrippedEntities;
using Eins.TransportEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.EventArgs
{
    public class CardDrawnEventArgs : IBaseEventArgs
    {
        public int Code { get; set; }

        public Player Player { get; set; }
        public IBaseCard Card { get; set; }

        public CardDrawnEventArgs(int code, IBasePlayer player, int orderID, IBaseCard card = null)
        {
            this.Code = code;
            this.Player = new Player
            {
                HeldCardAmount = player.HeldCards.Count,
                ConnectionID = player.ConnectionID,
                ID = player.ID,
                OrderID = orderID,
                Username = player.Username
            };
            this.Card = card;
        }
    }
}
