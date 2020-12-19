using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class Game
    {
        public List<GameRound> GameRounds { get; set; }

        public List<GamePlayer> Players { get; set; }

        public CardsPack CardsPack { get; set; }

        public Player CurrentMover { get; set; }
    }
}
