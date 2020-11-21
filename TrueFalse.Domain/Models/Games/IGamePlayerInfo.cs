using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public interface IGamePlayerInfo
    {
        int Priority { get; }

        Player Player { get; }

        IReadOnlyCollection<PlayingCard> Cards { get; }
    }
}
