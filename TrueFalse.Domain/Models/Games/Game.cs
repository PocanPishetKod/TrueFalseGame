using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameRules;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.Games
{
    public class Game
    {
        private StandartGameRules GameRules { get; set; }

        private ICollection<GameRound> Rounds { get; set; }

        public ICollection<GamePlayer> GamePlayers { get; set; }

        public Player Loser { get; internal set; }

        public CardsPack CardsPack { get; internal set; }

        public Player CurrentMover { get; internal set; }

        public bool IsCompleted => Loser != null;

        public Game(CardsPack cardsPack, IReadOnlyCollection<GameTablePlayer> players, StandartGameRules gameRules)
        {
            if (cardsPack == null)
            {
                throw new ArgumentNullException(nameof(cardsPack));
            }

            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));
            }

            GameRules = gameRules;
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
                    player.Cards.AddRange(CardsPack.TakeMany(cardPerPlayer));
                }
            }
        }

        public GameRound GetCurrentRound()
        {
            return Rounds.LastOrDefault();
        }

        public void Start()
        {
            CardsPack.Shuffle();
            DealCards();
        }

        public void MakeMove(IMove move)
        {

        }
    }
}
