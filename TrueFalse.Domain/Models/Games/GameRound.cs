using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;
using TrueFalse.Domain.Exceptions;

namespace TrueFalse.Domain.Models.Games
{
    public class GameRound
    {
        private readonly IList<IMove> _moves;

        public int MovesCount => _moves.Count;

        public Player Loser { get; private set; }

        public bool IsEnded => Loser != null;

        public GameRound()
        {
            _moves = new List<IMove>();
        }

        /// <summary>
        /// Возвращает все ходы указанного типа
        /// </summary>
        /// <typeparam name="TMove"></typeparam>
        /// <returns></returns>
        public IReadOnlyCollection<TMove> GetMoves<TMove>() where TMove : IMove
        {
            return _moves.OfType<TMove>().ToList();
        }

        public void AddMove(IMove move)
        {
            if (IsEnded)
            {
                throw new TrueFalseGameException("Раунд уже завершен");
            }

            _moves.Add(move);
        }

        public IMove GetLastMove()
        {
            return _moves.LastOrDefault();
        }

        /// <summary>
        /// Возвращает карты последнего хода, в котором бросали карты
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<PlayingCard> GetLastCards()
        {
            if (_moves.Count == 0)
            {
                return new List<PlayingCard>();
            }

            var index = !IsEnded ? _moves.Count - 1 : _moves.Count - 2;
            return (_moves[index] as MoveWithCards).Cards;
        }

        /// <summary>
        /// Возвращает объявленную в раунде стоимость карт
        /// </summary>
        /// <returns></returns>
        public PlayingCardRank GetRank()
        {
            if (_moves.Count == 0)
            {
                throw new TrueFalseGameException("В раунде еще не было ходов");
            }

            return (_moves[0] as FirstMove).Rank;
        }

        /// <summary>
        /// Возвращает все карты брошенные в этом раунде
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<PlayingCard> GetAllCards()
        {
            return GetMoves<MoveWithCards>()
                .SelectMany(m => m.Cards)
                .ToList();
        }

        /// <summary>
        /// Заканчивает раунд
        /// </summary>
        /// <param name="loser"></param>
        public void End(Player loser)
        {
            if (loser == null)
            {
                throw new ArgumentNullException(nameof(loser));
            }

            if (IsEnded)
            {
                throw new TrueFalseGameException("Раунд уже был завершен");
            }

            Loser = loser;
        }
    }
}
