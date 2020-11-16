using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;

namespace TrueFalse.Domain.Models.Moves
{
    /// <summary>
    /// Первый ход в раунде
    /// </summary>
    public class FirstMove : MoveWithCards
    {
        /// <summary>
        /// Объявляемая стоимость карт
        /// </summary>
        public PlayingCardRank Rank { get; private set; }

        public Guid InitiatorId { get; private set; }

        public FirstMove(IReadOnlyList<PlayingCard> cards, PlayingCardRank rank, Guid initiatorId)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            Cards = cards;
            Rank = rank;
            InitiatorId = initiatorId;
        }
    }
}
