using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;

namespace TrueFalse.Domain.Models.Moves
{
    /// <summary>
    /// Ход "Верю"
    /// </summary>
    public class BelieveMove : MoveWithCards
    {
        public BelieveMove(IReadOnlyList<PlayingCard> cards, Guid initiatorId)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            Cards = cards;
            InitiatorId = initiatorId;
        }
    }
}
