using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public class Game
    {
        private ICollection<GameRound> Rounds { get; set; }

        private ICollection<GamePlayer> GamePlayers { get; set; }

        public Player Loser { get; private set; }

        public CardsPack CardsPack { get; private set; }

        public Player CurrentMover { get; private set; }

        public bool IsCompleted => Loser != null;

        public bool IsStarted { get; private set; }

        public GameRound CurrentRound 
        { 
            get
            {
                var lastRound = Rounds.LastOrDefault();
                if (lastRound != null && lastRound.Loser == null)
                {
                    return lastRound;
                }

                return null;
            }
        }

        public Game(CardsPack cardsPack, IReadOnlyCollection<GameTablePlayer> players)
        {
            if (cardsPack == null)
            {
                throw new ArgumentNullException(nameof(cardsPack));
            }

            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));
            }

            CardsPack = cardsPack;
            Rounds = new List<GameRound>();
            SetPlayers(players);
        }

        /// <summary>
        /// Устанавливает игроков в порядке ходов
        /// </summary>
        /// <param name="players"></param>
        private void SetPlayers(IReadOnlyCollection<GameTablePlayer> players)
        {
            GamePlayers = players.OrderBy(p => p.GameTablePlaceNumber)
                .Select(p => new GamePlayer(p.Player, p.GameTablePlaceNumber))
                .ToList();

            CurrentMover = GamePlayers.First(p => p.Priority == GamePlayers.Min(pm => pm.Priority)).Player;
        }

        /// <summary>
        /// Раздает игрокам карты
        /// </summary>
        private void DealCards()
        {
            var cardPerPlayer = CardsPack.Count() / GamePlayers.Count;
            foreach (var player in GamePlayers)
            {
                for (int i = 0; i < cardPerPlayer; i += cardPerPlayer)
                {
                    player.GiveCards(CardsPack.TakeMany(cardPerPlayer));
                }
            }
        }

        private void NextRound()
        {
            Rounds.Add(new GameRound());
        }

        private bool ValidateFirstMove(FirstMove move)
        {
            if (move.Cards.Count > 4)
            {
                return false;
            }

            if (!CardsPack.IsRankContains(move.Rank))
            {
                return false;
            }

            return true;
        }

        public void Start()
        {
            CardsPack.Shuffle();
            DealCards();
            NextRound();
            IsStarted = true;
        }

        public void MakeFirstMove(FirstMove move)
        {
            if (!IsStarted)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            if (IsCompleted)
            {
                throw new TrueFalseGameException("Игра уже завершилась");
            }

            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            var gamePlayer = GamePlayers.FirstOrDefault(p => p.Player.Id == move.InitiatorId);
            if (gamePlayer == null)
            {
                throw new TrueFalseGameException($"Игрока с Id = {move.InitiatorId} нет за игровым столом");
            }

            if (!ValidateFirstMove(move))
            {
                throw new TrueFalseGameException("Не валидный ход");
            }

            gamePlayer.TakeCards(move.Cards.Select(c => c.Id).ToList());
            CurrentRound.AddMove(move);
        }

        public void MakeBeleiveMove(BelieveMove move)
        {

        }

        public void MakeDontBeleiveMove(DontBelieveMove move)
        {

        }
    }
}
