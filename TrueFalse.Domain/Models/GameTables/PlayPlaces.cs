using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PlayPlaces()
        {
            _seatedPlayers = new List<GameTablePlayer>();
        }

        /// <summary>
        /// Заполнена ли комната
        /// </summary>
        /// <returns></returns>
        public bool IsFull()
        {
            return _seatedPlayers.Count == PlacesCount;
        }

        /// <summary>
        /// Возвращает номер следующего игрового места
        /// </summary>
        /// <returns></returns>
        private int GetNextPlaceNumber()
        {
            return _seatedPlayers.Max(p => p.GameTablePlaceNumber);
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

            if (IsFull())
            {
                throw new Exception("Нет свободных игровых мест");
            }

            _seatedPlayers.Add(new GameTablePlayer(player, GetNextPlaceNumber()));
        }
    }
}
