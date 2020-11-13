using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameRules;
using TrueFalse.Domain.Models.Games;
using TrueFalse.Domain.Models.Moves;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    /// <summary>
    /// Игровой стол
    /// </summary>
    public abstract class GameTable
    {
        protected StandartGameRules GameRules { get; set; }

        protected PlayPlaces PlayPlaces { get; }

        protected Game CurrentGame { get; set; }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public Player Owner { get; protected set; }

        public IReadOnlyCollection<GameTablePlayer> Players => PlayPlaces.Players;

        public GameTable(Player owner, string name, Guid id)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            Id = id;
            Name = name;
            Owner = owner;
            GameRules = new StandartGameRules();
            PlayPlaces = CreatePlayPlaces();

            Join(owner);
        }

        protected abstract CardsPack CreateNewCardsPack();

        protected abstract PlayPlaces CreatePlayPlaces();

        /// <summary>
        /// Присоединяет пользователя к игровому столу
        /// </summary>
        /// <param name="player"></param>
        public void Join(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            PlayPlaces.PlantPlayer(player);
        }

        /// <summary>
        /// Убирает пользователя из игрового стола
        /// </summary>
        /// <param name="player"></param>
        public void Leave(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            PlayPlaces.RemovePlayer(player);
        }

        /// <summary>
        /// Запускает новую игру
        /// </summary>
        public void StartNewGame()
        {
            if (CurrentGame != null &&! CurrentGame.IsCompleted)
            {
                throw new TrueFalseGameException("Игра еще не окончена");
            }

            if (!PlayPlaces.IsFull())
            {
                throw new TrueFalseGameException("Не хватает игроков");
            }

            CurrentGame = new Game(CreateNewCardsPack(), Players, GameRules);
            CurrentGame.Start();
        }

        /// <summary>
        /// Обрабатывает ход
        /// </summary>
        /// <param name="move"></param>
        public void MakeMove(IMove move)
        {
            if (CurrentGame == null || CurrentGame.IsCompleted)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            if (!GameRules.CheckMove(move, this))
            {
                throw new TrueFalseGameException("Нарушение правил игры");
            }

            GameRules.ExecuteMove(move, this);
        }

        /// <summary>
        /// Опряделяет были ли уже ходы в раунде
        /// </summary>
        /// <returns></returns>
        public bool AlreadyMadeMovesInLastRound()
        {
            var lastRound = CurrentGame.GetCurrentRound();
            if (lastRound == null)
            {
                return false;
            }

            return lastRound.MovesCount > 0;
        }
    }
}
