using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Domain.Models.GameRules
{
    public interface ICheckingGameRule<TMove> : IGameRule<TMove> where TMove : IMove
    {
        bool Check(TMove move, GameTable gameTable);
    }
}
