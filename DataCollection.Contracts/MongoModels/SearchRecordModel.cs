using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Contracts.MongoModels
{
    public class SearchRecordModel : IActivityModelBase
    {   
        [BsonId]
        public ObjectId _Id { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
