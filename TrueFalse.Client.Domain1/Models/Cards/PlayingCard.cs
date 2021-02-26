using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Client.Domain.Models.Cards
{
    public class PlayingCard
    {
        public int Id { get; set; }

        public PlayingCardSuit? Suit { get; set; }

        public PlayingCardRank? Rank { get; set; }
    }
}
