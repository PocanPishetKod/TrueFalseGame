using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.GameTables
{
    public class GameTablePlayer
    {
        public Player Player { get; set; }

        public int GameTablePlaceNumber { get; set; }

        public GameTablePlayer(Player player, int placeNumber)
        {
            Player = player;
            GameTablePlaceNumber = placeNumber;
        }
    }
}
