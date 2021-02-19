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
        /// Выбранная карта на проверку
        /// </summary>
        public int SelectedCardId { get; private set; }

        public Guid InitiatorId { get; protected set; }

        public BelieveMove(int selectedCardId, Guid initiatorId)
        {
            SelectedCardId = selectedCardId;
            InitiatorId = initiatorId;
        }
    }
}
