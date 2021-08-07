using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Model
{
    public class CollectionBase
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string TenantName { get; set; }
        public string ConnectionKey { get; set; }
    }
}
