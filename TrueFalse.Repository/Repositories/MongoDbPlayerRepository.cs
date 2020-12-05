using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Domain.Interfaces.Repositories;
using TrueFalse.Domain.Models.Players;
using TrueFalse.Repository.MongoDb;

namespace TrueFalse.Repository.Repositories
{
    public class MongoDbPlayerRepository : IPlayerRepository
    {
        public MongoDbPlayerRepository(MongoDbContext mongoDbContext)
        {

        }

        public void Add(Player player)
        {
            throw new NotImplementedException();
        }

        public Player GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Player> GetPlayers()
        {
            throw new NotImplementedException();
        }

        public void Remove(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
