using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Domain.Models.GameRules
{
    public interface IExecutingGameRule<TMove> : IGameRule<TMove> where TMove : IMove
    {
        void Execute(TMove move, GameTable gameTable);
    }
}
