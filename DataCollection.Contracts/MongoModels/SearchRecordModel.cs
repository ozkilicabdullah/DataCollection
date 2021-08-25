﻿using MongoDB.Bson;
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
        public string SessionID { get; set; }
        public string UserID { get; set; }
        public string Value { get; set; }
        public string CreatedOn { get; set; }

    }
}
