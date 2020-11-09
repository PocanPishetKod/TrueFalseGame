using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    public class GameRound
    {
        private readonly ICollection<IMove> _moves;

        public int MovesCount => _moves.Count;

        public Player Loser { get; internal set; }

        public GameRound()
        {
            _moves = new List<IMove>();
        }

        public IReadOnlyCollection<TMove> GetMoves<TMove>() where TMove : IMove
        {
            return _moves.OfType<TMove>().ToList();
        }

        public void AddMove(IMove move)
        {
            _moves.Add(move);
        }
    }
}
