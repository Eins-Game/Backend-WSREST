using System;
using System.Collections.Generic;
using System.Text;

namespace Eins.TransportEntities.GameSession
{
    public class GamePlayer : Player
    {
        public bool ActivePlayer { get; set; } = false;

        //TODO: Replace with card object
        public List<string> HeldCards { get; set; } = new List<string>();

        /// <summary>
        /// Only used for deserialization
        /// </summary>
        public GamePlayer() {}

        /// <summary>
        /// Create GamePlayer from Lobby Player
        /// </summary>
        /// <param name="player">Player to be converted</param>
        public GamePlayer(Player player)
        {
            this.PlayerID = player.PlayerID;
            this.Username = player.Username;
            this.CreationDate = player.CreationDate;
        }
    }
}
