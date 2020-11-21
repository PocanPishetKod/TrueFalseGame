﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Cards;
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
        protected PlayPlaces PlayPlaces { get; }

        protected Game CurrentGame { get; set; }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public Player Owner { get; protected set; }

        public IReadOnlyCollection<GameTablePlayer> Players => PlayPlaces.Players;

        public bool IsFull => PlayPlaces.IsFull;

        /// <summary>
        /// Является ли комната прогодна для использования
        /// </summary>
        public bool IsInvalid => Owner == null;

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
            PlayPlaces = CreatePlayPlaces();

            Join(owner);
        }

        /// <summary>
        /// Возвращает следующего владельца комнаты
        /// </summary>
        /// <returns></returns>
        private GameTablePlayer GetNextOwner()
        {
            if (Players.Count == 0)
            {
                return null;
            }

            return Players.First(p => p.GameTablePlaceNumber == Players.Min(gp => gp.GameTablePlaceNumber));
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

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
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

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            PlayPlaces.RemovePlayer(player);
            if (player.Id == Owner.Id)
            {
                Owner = GetNextOwner()?.Player;
            }
        }

        /// <summary>
        /// Запускает новую игру
        /// </summary>
        public void StartNewGame()
        {
            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            if (CurrentGame != null &&! CurrentGame.IsEnded)
            {
                throw new TrueFalseGameException("Игра еще не окончена");
            }

            if (!PlayPlaces.IsFull)
            {
                throw new TrueFalseGameException("Не хватает игроков");
            }

            CurrentGame = new Game(CreateNewCardsPack(), Players);
            CurrentGame.Start();
        }

        /// <summary>
        /// Делает ход "Первый ход"
        /// </summary>
        /// <param name="move"></param>
        public void MakeFirstMove(FirstMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            CurrentGame.MakeFirstMove(move);
        }

        /// <summary>
        /// Делает ход "Верю"
        /// </summary>
        /// <param name="move"></param>
        public void MakeBeleiveMove(BelieveMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            CurrentGame.MakeBeleiveMove(move);
        }

        /// <summary>
        /// Делает ход "Не верю"
        /// </summary>
        /// <param name="move"></param>
        public void MakeDontBeleiveMove(DontBelieveMove move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            CurrentGame.MakeDontBeleiveMove(move);
        }
    }
}
