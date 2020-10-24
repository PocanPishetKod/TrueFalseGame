using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;

namespace TrueFalse.Repository.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private static List<Player> _players;

        static PlayerRepository()
        {
            _players = new List<Player>();
        }

        public void Add(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            _players.Add(player);
        }

        public Player GetById(Guid id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public IReadOnlyCollection<Player> GetPlayers()
        {
            return _players;
        }

        public void Remove(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var item = GetById(player.Id);
            if (item == null)
            {
                throw new NullReferenceException($"Удаляемый пользователь с Id = {player.Id} не существует либо уже был удален");
            }

            _players.Remove(item);
        }
    }
}
