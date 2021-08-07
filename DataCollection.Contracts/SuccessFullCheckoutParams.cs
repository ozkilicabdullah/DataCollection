using DataCollection.Contracts.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class SuccessFullCheckoutParams : ModelActivityBase
    {
        public string OrderId { get; set; }
        public string CampaignId { get; set; }
        public string CouponId { get; set; }
        public string DeliveryType { get; set; }
        public bool IsFreeShipping { get; set; }
        public string DeliveryAddressId { get; set; }
        public string Platform { get; set; }
        public string PaymentTypeId { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
    }
}
