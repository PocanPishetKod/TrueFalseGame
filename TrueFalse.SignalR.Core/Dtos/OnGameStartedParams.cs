using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class OnGameStartedParams
    {
        public Guid MoverId { get; set; }

        public List<PlayerCardsInfoDto> PlayerCardsInfo { get; set; }
    }
}
