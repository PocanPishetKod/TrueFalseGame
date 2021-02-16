using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos.Results
{
    public class MakeDontBeliveMoveResult
    {
        public Guid GameTableId { get; set; }

        public Guid? NextMoverId { get; set; }

        public PlayingCardDto CheckedCard { get; set; }

        public Guid LoserId { get; set; }

        public IReadOnlyCollection<PlayingCardDto> TakedLoserCards { get; set; }

        public IReadOnlyCollection<MoveType> NextPossibleMoves { get; set; }
    }
}
