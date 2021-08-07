using DataCollection.Contracts.Entites;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataCollection.Contracts.MongoModels
{
    public class WishRecordModel : IActivityModelBase
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string Type { get; set; }
        public WishInfo WishInfo { get; set; }
        public string SessionId { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
