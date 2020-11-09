using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Moves
{
    /// <summary>
    /// Карточный ход
    /// </summary>
    public interface IMove
    {
        /// <summary>
        /// Идентификатор игрока, сделавшего ход
        /// </summary>
        Guid InitiatorId { get; }
    }
}
