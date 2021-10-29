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
        public string UserID { get; set; }
        public string ProductID { get; set; }
        public string Type { get; set; }
        public WishInfo WishInfo { get; set; }
        public string SessionID { get; set; }
        public string CreatedOn { get; set; }
        public string AppKey { get; set; }

    }
}
