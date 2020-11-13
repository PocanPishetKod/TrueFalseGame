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
        /// Идентификатор карты уникальный на уровне комнаты
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Масть
        /// </summary>
        public PlayingCardSuit Suit { get; private set; }

        /// <summary>
        /// Достоинство
        /// </summary>
        public PlayingCardRank Rank { get; set; }

        public PlayingCard(int id, PlayingCardSuit suit, PlayingCardRank rank)
        {
            Id = id;
            Suit = suit;
            Rank = rank;
        }
    }
}
