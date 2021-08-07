using DataCollection.Contracts.Entites;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace DataCollection.Contracts.MongoModels
{
    public class BasketRecordModel : IActivityModelBase
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string ProductId { get; set; }
        public string Type { get; set; }
        public BasketInfo BasketInfo { get; set; }


    }
}
