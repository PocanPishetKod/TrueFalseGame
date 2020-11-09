using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameContext
    {
        public Player Loser { get; internal set; }

        public CardsPack CardsPack { get; internal set; }

        public Player CurrentMover { get; internal set; }

        public PlayPlaces Players { get; set; }

        public ICollection<GameRound> Rounds { get; internal set; }

        public bool IsCompleted => Loser != null;

        public GameContext(CardsPack cardsPack)
        {
            if (cardsPack == null)
            {
                throw new ArgumentNullException(nameof(cardsPack));
            }

            CardsPack = cardsPack;
            Rounds = new List<GameRound>();
        }

        public bool AlreadyMadeMoves()
        {
            var currentRound = Rounds.LastOrDefault();
            if (currentRound == null)
            {
                return false;
            }

            return currentRound.MovesCount > 0;
        }
    }
}
