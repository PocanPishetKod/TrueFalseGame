using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;

namespace TrueFalse.Domain.Models.Moves
{
    public abstract class MoveWithCards : IMove
    {
        /// <summary>
        /// Подкидываемые карты
        /// </summary>
        public IReadOnlyList<PlayingCard> Cards { get; protected set; }

        public Guid InitiatorId { get; protected set; }
    }
}
