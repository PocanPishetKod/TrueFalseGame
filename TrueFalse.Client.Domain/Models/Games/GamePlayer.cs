using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class GamePlayer
    {
        public Player Player { get; set; }

        public int Priority { get; set; }

        public List<PlayingCard> PlayingCards { get; set; }
    }
}
