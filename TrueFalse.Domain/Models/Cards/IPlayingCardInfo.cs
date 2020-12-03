using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models.Cards
{
    public interface IPlayingCardInfo
    {
        int Id { get; }

        PlayingCardSuit Suit { get; }

        PlayingCardRank Rank { get; set; }
    }
}
