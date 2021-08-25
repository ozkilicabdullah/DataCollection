using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Contracts.MongoModels
{
    public class ReturnedRecordModel : IActivityModelBase
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string SessionID { get; set; }
        public string UserID { get; set; }
        public bool PartialRefund { get; set; }
        public List<string> ProductID { get; set; }
        public string CreatedOn { get; set; }

    }
}
