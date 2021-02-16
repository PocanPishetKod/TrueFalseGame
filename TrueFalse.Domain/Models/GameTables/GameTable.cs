using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public abstract class GameTable : IDisposable
    {
        private bool _isDisposed;
        private readonly Mutex _joinAndLeaveMutex;
        private readonly Mutex _moveMutex;

        protected PlayPlaces PlayPlaces { get; }

        protected Game CurrentGame { get; set; }

        public DateTime DateOfCreate { get; protected set; }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public Player Owner { get; protected set; }

        public IReadOnlyCollection<GameTablePlayer> Players => PlayPlaces.Players;

        public bool IsFull => PlayPlaces.IsFull;

        public bool GameInProgress => CurrentGame != null ? CurrentGame.IsStarted && !CurrentGame.IsEnded : false;

        /// <summary>
        /// Является ли комната прогодна для использования
        /// </summary>
        public bool IsInvalid => Owner == null;

        public Player CurrentMover => CurrentGame?.CurrentMover;

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
            DateOfCreate = DateTime.Now;
            PlayPlaces = CreatePlayPlaces();

            Join(owner);

            _joinAndLeaveMutex = new Mutex();
            _moveMutex = new Mutex();
            _isDisposed = false;
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

            try
            {
                _joinAndLeaveMutex.WaitOne();

                if (IsInvalid)
                {
                    throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
                }

                if (CurrentGame != null && CurrentGame.IsStarted && !CurrentGame.IsEnded)
                {
                    throw new TrueFalseGameException("Игра уже началась");
                }

                PlayPlaces.PlantPlayer(player);
            }
            finally
            {
                _joinAndLeaveMutex.ReleaseMutex();
            }
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

            try
            {
                _joinAndLeaveMutex.WaitOne();

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
            finally
            {
                _joinAndLeaveMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Запускает новую игру
        /// </summary>
        public void StartNewGame(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (IsInvalid)
            {
                throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
            }

            if (CurrentGame != null &&! CurrentGame.IsEnded)
            {
                throw new TrueFalseGameException("Игра еще не окончена");
            }

            if (player.Id != Owner.Id)
            {
                throw new TrueFalseGameException("Только владелец игрового стола может запустить игру");
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

            try
            {
                _moveMutex.WaitOne();

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
            finally
            {
                _moveMutex.ReleaseMutex();
            }
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

            try
            {
                _moveMutex.WaitOne();

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
            finally
            {
                _moveMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Делает ход "Не верю"
        /// </summary>
        /// <param name="move"></param>
        public void MakeDontBeleiveMove(DontBelieveMove move, out IReadOnlyCollection<IPlayingCardInfo> takedLoserCards, out Guid loserId)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            try
            {
                _moveMutex.WaitOne();

                if (IsInvalid)
                {
                    throw new TrueFalseGameException("Игровой стол находится в инвалидном состоянии");
                }

                if (CurrentGame == null)
                {
                    throw new TrueFalseGameException("Игра еще не началась");
                }

                CurrentGame.MakeDontBeleiveMove(move, out takedLoserCards, out loserId);
            }
            finally
            {
                _moveMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Возвращает карты игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="cardIds"></param>
        /// <returns></returns>
        public IReadOnlyCollection<IPlayingCardInfo> GetPlayerCards(Guid playerId, IReadOnlyCollection<int> cardIds = null)
        {
            if (cardIds == null)
            {
                throw new ArgumentNullException(nameof(cardIds));
            }

            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            return CurrentGame.GetPlayerCards(playerId, cardIds);
        }

        /// <summary>
        /// Возвращает карту из ходов текущего раунда по идентификатору
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public IPlayingCardInfo GetCardFromCurrentRoundById(int cardId)
        {
            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            return CurrentGame.GetCardFromCurrentRoundById(cardId);
        }

        /// <summary>
        /// Возвращает список типов возможных ходов
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<Type> GetNextPossibleMoves()
        {
            if (CurrentGame == null)
            {
                throw new TrueFalseGameException("Игра еще не началась");
            }

            return CurrentGame.GetNextPossibleMoves();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_joinAndLeaveMutex != null)
                {
                    _joinAndLeaveMutex.Dispose();
                }

                if (_moveMutex != null)
                {
                    _moveMutex.Dispose();
                }

                _isDisposed = true;
            }
        }
    }
}
