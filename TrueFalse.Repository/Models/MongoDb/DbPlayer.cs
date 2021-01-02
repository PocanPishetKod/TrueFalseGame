using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Repository.Models.MongoDb
{
    public class DbPlayer
    {
        public const string CollectionName = "players";

        [BsonId]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
