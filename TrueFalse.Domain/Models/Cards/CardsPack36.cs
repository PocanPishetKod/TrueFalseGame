using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Cards
{
    public class CardsPack36 : CardsPack
    {
        public override bool IsRankContains(PlayingCardRank rank)
        {
            return rank >= PlayingCardRank.Six;
        }

        protected override void CreateCards()
        {
            _cards = new List<PlayingCard>(36);
            CreateCards(PlayingCardRank.Six);
        }
    }
}
