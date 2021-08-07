using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace DataCollection.Contracts.Entites.Base
{
    public class CollectionBaseModel
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string TenantName { get; set; } // Database Name
        public string ConnectionKey { get; set; } // Collection (Table) Name
    }
}
