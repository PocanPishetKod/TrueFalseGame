using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models;

namespace TrueFalse.Repository.Repositories
{
    public class GameTableRepository : IGameTableRepository
    {
        private static List<GameTable> _gameTables;

        static GameTableRepository()
        {
            _gameTables = new List<GameTable>();
        }

        public void Add(GameTable gameTable)
        {
            if (gameTable == null)
            {
                throw new ArgumentNullException(nameof(gameTable));
            }

            _gameTables.Add(gameTable);
        }

        public GameTable GetById(Guid id)
        {
            return _gameTables.FirstOrDefault(t => t.Id == id);
        }

        public GameTable GetByOwner(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            return _gameTables.FirstOrDefault(t => t.Owner.Id == player.Id);
        }

        public GameTable GetByPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            return _gameTables.FirstOrDefault(t => t.Players.Any(p => p.Player.Id == player.Id));
        }

        public IReadOnlyCollection<GameTable> GetGameTables()
        {
            return _gameTables;
        }

        public void Remove(GameTable gameTable)
        {
            if (gameTable == null)
            {
                throw new ArgumentNullException(nameof(gameTable));
            }

            var item = _gameTables.FirstOrDefault(t => t.Id == gameTable.Id);
            if (item == null)
            {
                throw new NullReferenceException($"Удаляемый игровой стол с Id = {gameTable.Id} не существует либо уже был удален");
            }

            _gameTables.Remove(item);
        }

        public void Update(GameTable gameTable)
        {
            
        }
    }
}
