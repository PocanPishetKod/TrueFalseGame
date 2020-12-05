using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models.Players;
using TrueFalse.Repository.Models.MongoDb;
using TrueFalse.Repository.MongoDb;

namespace TrueFalse.Repository.Repositories
{
    public class MongoDbPlayerRepository : IPlayerRepository
    {
        private readonly MongoDbContext _mongoDbContext;

        public MongoDbPlayerRepository(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public void Add(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            _mongoDbContext.PlayerCollection.InsertOne(new DbPlayer()
            {
                Id = player.Id,
                Name = player.Name
            });
        }

        public Player GetById(Guid id)
        {
            var player = _mongoDbContext.PlayerCollection.Find(p => p.Id == id).FirstOrDefault();
            if (player == null)
            {
                return null;
            }

            return new Player(player.Id, player.Name);
        }

        public IReadOnlyCollection<Player> GetPlayers()
        {
            var players = _mongoDbContext.PlayerCollection.Find(p => true).ToList();
            return players.Select(p => new Player(p.Id, p.Name)).ToList();
        }

        public void Remove(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var deleteResult = _mongoDbContext.PlayerCollection.DeleteOne(p => p.Id == player.Id);
        }
    }
}
