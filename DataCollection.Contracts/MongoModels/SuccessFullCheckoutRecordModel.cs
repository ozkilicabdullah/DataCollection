using DataCollection.Contracts.Entites;
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
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string CampaignId { get; set; }
        public string CouponId { get; set; }
        public string DeliveryType { get; set; }
        public bool IsFreeShipping { get; set; }
        public string DeliveryAddressId { get; set; }
        public string Platform { get; set; }
        public string PaymentTypeId { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
