using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Application.Dtos;

namespace TrueFalse.SignalR.Core.Dtos
{
    public class PlayerCardsInfoDto
    {
        public Guid PlayerId { get; set; }

        public int CardsCount { get; set; }

        public List<PlayingCardDto> Cards { get; set; }
    }
}
