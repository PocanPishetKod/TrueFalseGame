using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Exceptions;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    /// <summary>
    /// Игровые места
    /// </summary>
    public abstract class PlayPlaces
    {
        protected List<GameTablePlayer> _seatedPlayers;

        /// <summary>
        /// Количество игровых мест
        /// </summary>
        protected abstract int PlacesCount { get; }

        public IReadOnlyCollection<GameTablePlayer> Players => _seatedPlayers;

        /// <summary>
        /// Заполнена ли комната
        /// </summary>
        public bool IsFull => _seatedPlayers.Count == PlacesCount;

        public PlayPlaces()
        {
            _seatedPlayers = new List<GameTablePlayer>();
        }

        /// <summary>
        /// Проверяет занятость места пользователем
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool PlayerAlreadySeated(Player player)
        {
            return _seatedPlayers.Any(p => p.Player.Id == player.Id);
        }

        /// <summary>
        /// Возвращает номер следующего игрового места
        /// </summary>
        /// <returns></returns>
        protected int GetNextPlaceNumber()
        {
            if (_seatedPlayers.Count == 0)
            {
                return 1;
            }

            return _seatedPlayers.Max(p => p.GameTablePlaceNumber) + 1;
        }

        /// <summary>
        /// Усаживает игрока за игровое место
        /// </summary>
        /// <param name="player"></param>
        public void PlantPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (IsFull)
            {
                throw new TrueFalseGameException("Нет свободных игровых мест");
            }

            if (PlayerAlreadySeated(player))
            {
                throw new TrueFalseGameException($"Пользователь с Id = {player.Id} уже сидит за столом");
            }

            _seatedPlayers.Add(new GameTablePlayer(player, GetNextPlaceNumber()));
        }

        /// <summary>
        /// Удаляет игрока с его игрового места
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (!PlayerAlreadySeated(player))
            {
                throw new TrueFalseGameException($"Пользователя с Id = {player.Id} нет за столом");
            }

            var place = _seatedPlayers.First(p => p.Player.Id == player.Id);
            _seatedPlayers.Remove(place);
        }
    }
}
