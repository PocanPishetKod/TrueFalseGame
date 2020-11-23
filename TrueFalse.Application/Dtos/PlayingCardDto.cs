using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Application.Dtos
{
    public class PlayingCardDto
    {
        public int Id { get; private set; }

        public int Suit { get; private set; }

        public int Rank { get; set; }
    }
}
