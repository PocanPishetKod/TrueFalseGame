using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Cards
{
    /// <summary>
    /// Игровая карта
    /// </summary>
    public class PlayingCard
    {
        /// <summary>
        /// Масть
        /// </summary>
        public PlayingCardSuit Suit { get; private set; }

        /// <summary>
        /// Достоинство
        /// </summary>
        public PlayingCardRank Rank { get; set; }

        public PlayingCard(PlayingCardSuit suit, PlayingCardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}
