using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Domain.Models.Cards;

namespace TrueFalse.Application.Dtos.Results
{
    public class PlayerCardsInfo
    {
        public Guid PlayerId { get; set; }

        public int CardsCount { get; set; }

        public List<PlayingCardDto> Cards { get; set; }
    }
}
