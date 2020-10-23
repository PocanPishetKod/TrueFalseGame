using System;
using System.Collections.Generic;
using System.Text;

namespace TrueFalse.Domain.Models
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
        }

        protected abstract bool CanJoinPlayer();
    }
}
