using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class OnBeliveMoveMadeParams
    {
        public Guid NextMoverId { get; set; }

        public Guid MoverId { get; set; }

        public Guid LoserId { get; set; }

        public PlayingCardDto CheckedCard { get; set; }

        public List<int> HiddenTakedLoserCards { get; set; }

        public List<PlayingCardDto> TakedLoserCards { get; set; }

        public List<MoveType> NextPossibleMoves { get; set; }
    }
}
