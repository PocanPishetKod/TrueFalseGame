using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFalse.Repository.Models.MongoDb
{
    public class Player
    {
        public const string CollectionName = "Players";

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
