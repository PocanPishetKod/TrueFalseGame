﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Moves
{
    /// <summary>
    /// Ход "Не верю"
    /// </summary>
    public class DontBelieveMove : IMove
    {
        /// <summary>
        /// Выбранная карта на проверку
        /// </summary>
        public int SelectedCardId { get; private set; }

        public Guid InitiatorId { get; private set; }

        public DontBelieveMove(int selectedCardId, Guid initiatorId)
        {
            SelectedCardId = selectedCardId;
            InitiatorId = initiatorId;
        }
    }
}
