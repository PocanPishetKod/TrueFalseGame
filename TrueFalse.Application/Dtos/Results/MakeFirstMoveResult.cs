using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos.Results
{
    public class MakeFirstMoveResult
    {
        public Guid GameTableId { get; set; }

        public Guid NextMoverId { get; set; }

        public IReadOnlyCollection<MoveType> NextPossibleMoves { get; set; }
    }
}
