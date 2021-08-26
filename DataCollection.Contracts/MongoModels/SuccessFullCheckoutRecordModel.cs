﻿using DataCollection.Contracts.Entites;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Contracts.MongoModels
{
    public class SuccessFullCheckoutRecordModel : IActivityModelBase
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string SessionID { get; set; }
        public string UserID { get; set; }
        public string OrderID { get; set; }
        public string CampaignID { get; set; }
        public string CouponID { get; set; }
        public string DeliveryType { get; set; }
        public bool IsFreeShipping { get; set; }
        public string DeliveryAddressID { get; set; }
        public string Platform { get; set; }
        public string PaymentTypeID { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
        public string CreatedOn { get; set; }

    }
}
