using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Models.Cards;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Domain.Models.GameTables
{
    /// <summary>
    /// Игровой стол
    /// </summary>
    public abstract class GameTable
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public Player Owner { get; protected set; }

        public ICollection<GameTablePlayer> Players { get; set; }

        protected abstract PlayPlaces PlayPlaces { get; }

        protected abstract CardsPack CardsPack { get; }

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

            Initialize();

            Join(owner);
        }

        protected abstract void Initialize();

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
    }
}
