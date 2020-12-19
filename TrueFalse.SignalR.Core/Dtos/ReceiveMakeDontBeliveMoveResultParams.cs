using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class ReceiveMakeDontBeliveMoveResultParams : ResponseParams
    {
        public Guid? NextMoverId { get; set; }

        public Guid? LoserId { get; set; }

        public PlayingCardDto CheckedCard { get; set; }

        public List<int> HiddenTakedLoserCards { get; set; }

        public List<PlayingCardDto> TakedLoserCards { get; set; }
    }
}
