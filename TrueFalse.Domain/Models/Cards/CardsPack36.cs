using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Cards
{
    public class CardsPack36 : CardsPack
    {
        protected override void CreateCards()
        {
            _cards = new List<PlayingCard>(36);
            CreateCards(PlayingCardRank.Six);
        }
    }
}
