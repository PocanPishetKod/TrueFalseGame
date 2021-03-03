using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class GameRound
    {
        private List<Move> _moves;
        public IReadOnlyCollection<Move> Moves => _moves;

        public Player Loser { get; private set; }

        public bool IsEnded => Loser != null;

        public GameRound()
        {
            _moves = new List<Move>();
        }

        public void AddMove(Move move)
        {
            if (IsEnded)
            {
                return;
            }

            _moves.Add(move);
        }

        public void End(Player loser)
        {
            Loser = loser;
        }
    }
}
