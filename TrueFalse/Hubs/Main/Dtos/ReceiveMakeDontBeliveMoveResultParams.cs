using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.Hubs.Main.Dtos
{
    public class ReceiveMakeDontBeliveMoveResultParams
    {
        public bool Succeeded { get; set; }

        public Guid? NextMoverId { get; set; }

        public Guid? LoserId { get; set; }

        public PlayingCardDto CheckedCard { get; set; }

        public List<int> HiddenTakedLoserCards { get; set; }

        public List<PlayingCardDto> TakedLoserCards { get; set; }
    }
}
