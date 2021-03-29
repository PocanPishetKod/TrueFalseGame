using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Client.Domain.Exceptions;
using TrueFalse.Client.Domain.Models.Cards;
using TrueFalse.Client.Domain.Models.GameTables;
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

        public GameRound CurrentRound => IsEnded ? null : GameRounds.LastOrDefault();

        public CardsPack CardsPack { get; private set; }

        public Player CurrentMover { get; private set; }

        private List<GamePlayer> _players;
        public IReadOnlyCollection<GamePlayer> Players => _players;

        private Player _loser;
        public Player Loser => _loser;

        public bool IsEnded => _loser != null;

        public Game(IReadOnlyCollection<GameTablePlayer> players)
        {
            _nextPossibleMoves = new List<MoveType>();

            _players = new List<GamePlayer>(players.Select(p => new GamePlayer() 
            {
                Player = p.Player,
                Priority = p.GameTablePlaceNumber,
                PlayingCards = new List<PlayingCard>()
            }));
        }

        private void NextRound()
        {
            GameRounds.Add(new GameRound());
        }

        private void End(GamePlayer gamePlayer)
        {
            _loser = gamePlayer.Player;
            CurrentMover = null;
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

        public void End()
        {
            throw new NotImplementedException();
        }

        public bool CanMakeMove(MoveType moveType)
        {
            return _nextPossibleMoves.Contains(moveType);
        }

        public void MakeFirstMove(FirstMove move, Player nextMover)
        {
            CurrentRound.AddMove(move);
            CurrentMover = nextMover;
        }

        public void MakeBeliveMove(BeliveMove move, Guid nextMoverId, Guid loserId, IReadOnlyCollection<PlayingCard> takedLoserCards)
        {
            var loser = Players.FirstOrDefault(p => p.Player.Id == loserId);
            loser.PlayingCards.AddRange(takedLoserCards);

            CurrentRound.AddMove(move);
            CurrentRound.End(loser.Player);

            NextRound();

            CurrentMover = Players.First(p => p.Player.Id == nextMoverId).Player;
        }

        public void MakeDontBeliveMove(DontBeliveMove move, Guid? nextMoverId, Guid loserId, IReadOnlyCollection<PlayingCard> takedLoserCards)
        {
            var loser = Players.FirstOrDefault(p => p.Player.Id == loserId);
            loser.PlayingCards.AddRange(takedLoserCards);

            CurrentRound.AddMove(move);
            CurrentRound.End(loser.Player);

            if (nextMoverId.HasValue)
            {
                NextRound();
                CurrentMover = Players.First(p => p.Player.Id == nextMoverId).Player;
            } 
            else
            {
                End(loser);
            }
        }
    }
}
