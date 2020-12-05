using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFalse.Repository.Models.MongoDb;

namespace TrueFalse.Repository.MongoDb
{
    public class MongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public MongoDbContext(MongoDbSettings mongoDbSettings)
        {
            _mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            _database = _mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        }

        public IMongoCollection<DbPlayer> PlayerCollection => _database.GetCollection<DbPlayer>(DbPlayer.CollectionName);
    }
}
