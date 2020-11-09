using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Moves;

namespace TrueFalse.Domain.Models.GameRules
{
    public interface IGameRule<TMove> where TMove : IMove
    {
    }
}
