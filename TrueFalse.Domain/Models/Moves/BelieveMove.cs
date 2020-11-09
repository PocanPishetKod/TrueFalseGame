using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;

namespace TrueFalse.Domain.Models.Moves
{
    /// <summary>
    /// Ход "Верю"
    /// </summary>
    public class BelieveMove : IMove
    {
        /// <summary>
        /// Подкидываемые карты
        /// </summary>
        public IReadOnlyList<PlayingCard> Cards { get; private set; }

        public Guid InitiatorId { get; private set; }

        public BelieveMove(IReadOnlyList<PlayingCard> cards, Guid initiatorId)
        {
            if (Cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            Cards = cards;
            InitiatorId = initiatorId;
        }
    }
}
