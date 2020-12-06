using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.SignalR.Client.Dtos
{
    public class PlayingCardDto
    {
        public int Id { get; set; }

        public int Suit { get; set; }

        public int Rank { get; set; }
    }
}
