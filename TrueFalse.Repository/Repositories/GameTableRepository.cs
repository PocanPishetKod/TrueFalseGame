using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models.GameTables;
using TrueFalse.Domain.Models.Players;

namespace TrueFalse.Repository.Repositories
{
    public class GameTableRepository : IGameTableRepository
    {
        private static ConcurrentDictionary<Guid, GameTable> _gameTables;

        static GameTableRepository()
        {
            _gameTables = new ConcurrentDictionary<Guid, GameTable>();
        }

        public void Add(GameTable gameTable)
        {
            if (gameTable == null)
            {
                throw new ArgumentNullException(nameof(gameTable));
            }

            if (!_gameTables.TryAdd(gameTable.Id, gameTable))
            {
                throw new Exception("Такой ключ уже есть");
            }
        }

        public GameTable GetById(Guid id)
        {
            if (_gameTables.TryGetValue(id, out var result))
            {
                return result;
            }

            return null;
        }

        public GameTable GetByOwner(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            return _gameTables.FirstOrDefault(t => t.Value.Owner.Id == player.Id).Value;
        }

        public GameTable GetByPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            return _gameTables.FirstOrDefault(t => t.Value.Players.Any(p => p.Player.Id == player.Id)).Value;
        }

        public IReadOnlyCollection<GameTable> GetGameTables(int pageNum, int perPage)
        {
            return _gameTables.OrderByDescending(item => item.Value.DateOfCreate)
                .Skip(perPage * (pageNum - 1))
                .Take(perPage)
                .Select(item => item.Value)
                .ToList();
        }

        public void Remove(GameTable gameTable)
        {
            if (gameTable == null)
            {
                throw new ArgumentNullException(nameof(gameTable));
            }

            if (!_gameTables.ContainsKey(gameTable.Id))
            {
                throw new Exception($"Игрового стола с Id = {gameTable.Id} нет в списке");
            }

            _gameTables.TryRemove(gameTable.Id, out var result);
        }

        public void Update(GameTable gameTable)
        {
            
        }
    }
}
