using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.Moves;
using TrueFalse.Client.Domain.Models.Players;
using TrueFalse.SignalR.Client.Dtos;

namespace TrueFalse.Client.Domain.Models.Games
{
    public class Game
    {
        private List<MoveType> _nextPossibleMoves;
        public IReadOnlyCollection<MoveType> NextPossibleMoves => _nextPossibleMoves;

        public List<GameRound> GameRounds { get; private set; }

        public GameRound CurrentRound => GameRounds.LastOrDefault();

        public CardsPack CardsPack { get; private set; }

        public Player CurrentMover { get; private set; }

        public Game()
        {
            _nextPossibleMoves = new List<MoveType>();
        }

        public void SetNextPossibleMoves(IReadOnlyCollection<MoveType> moveTypes)
        {
            _nextPossibleMoves.Clear();
            _nextPossibleMoves.AddRange(moveTypes);
        }

        public void Start(int cardsCount, Player mover)
        {
            CardsPack = new CardsPack(cardsCount);
            CurrentMover = mover;
            GameRounds = new List<GameRound>() { new GameRound() };
        }

        public void MakeFirstMove(FirstMove move, Player nextMover)
        {
            CurrentRound.Moves.Add(move);
            CurrentMover = nextMover;
        }

        public void MakeBeliveMove(BeliveMove move, Player nextMover)
        {

        }

        public void MakeDontBeliveMove(DontBeliveMove move, Player nextMover)
        {

        }
    }
}
