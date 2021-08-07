using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataCollection.Contracts.MongoModels
{
    public interface IRecordModelBase
    {
        [BsonId]
        ObjectId _Id { get; set; }
    }
    public interface IActivityModelBase : IRecordModelBase
    {
        string SessionId { get; set; }
        string UserId { get; set; }
    }
}
