using System;
using System.Collections.Generic;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public class GamePlayer
    {
        public int Priority { get; set; }

        public Player Player { get; private set; }

        public List<PlayingCard> Cards { get; private set; }

        public GamePlayer(Player player, int priority)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            Player = player;
            Priority = priority;
            Cards = new List<PlayingCard>();
        }
    }
}
