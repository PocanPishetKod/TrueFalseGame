using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Domain.Models.GameRules.FirstMoveRules
{
    public class CanMakeFirstMove : ICheckingGameRule<FirstMove>
    {
        public bool Check(FirstMove move, GameTable gameTable)
        {
            return !gameTable.AlreadyMadeMovesInLastRound();
        }
    }
}
