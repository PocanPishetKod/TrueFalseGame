using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.GameRules;
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

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public Player Owner { get; protected set; }

        public GameContext CurrentGame { get; private set; }

        public ICollection<GameTablePlayer> Players { get; set; }

        protected abstract PlayPlaces PlayPlaces { get; }

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

            Initialize();

            Join(owner);
        }

        protected abstract void Initialize();

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

            if (!CurrentGame.Players.IsFull())
            {
                throw new TrueFalseGameException("Не хватает игроков");
            }

            CurrentGame = new GameContext(CreateNewCardsPack());
        }

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

        public bool AlreadyMadeMove()
        {

        }
    }
}
