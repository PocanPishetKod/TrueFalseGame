using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Cards
{
    public class CardsPack52 : CardsPack
    {
        protected override void CreateCards()
        {
            _cards = new List<PlayingCard>(52);
            CreateCards(PlayingCardRank.Two);
        }
    }
}
