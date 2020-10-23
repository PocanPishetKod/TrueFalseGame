using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
{
    /// <summary>
    /// Игрок, находящийся за игровым столом
    /// </summary>
    public class GameTablePlayer
    {
        /// <summary>
        /// Игрок
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// Место игрока за игровым столом
        /// </summary>
        public int GameTablePlaceNumber { get; private set; }

        public GameTablePlayer(Player player, int gameTablePlaceNumber)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (gameTablePlaceNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gameTablePlaceNumber));
            }

            Player = player;
            GameTablePlaceNumber = gameTablePlaceNumber;
        }
    }
}
