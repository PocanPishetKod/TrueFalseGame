using System;
using System.Collections.Generic;
using System.Linq;
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
            Players = new List<GameTablePlayer>();

            Join(owner);
        }

        private int GetNextPositionNumber()
        {
            if (Players.Count == 0)
            {
                return 1;
            }

            return Players.Max(p => p.GameTablePlaceNumber) + 1;
        }

        protected abstract bool CanJoinPlayer();

        public void Join(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (CanJoinPlayer())
            {
                throw new Exception($"К столу с Id = {Id} нельзя присоединиться");
            }

            if (Players.Any(p => p.Player.Id == player.Id))
            {
                throw new Exception($"Игрок с Id = {player.Id} уже находится за столом с Id = {Id}");
            }

            Players.Add(new GameTablePlayer(player, GetNextPositionNumber()));
        }

        public void Leave(Player player)
        {

        }
    }
}
