using DataCollection.Contracts.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class SuccessFullCheckoutParams : ModelActivityBase
    {
        public string OrderID { get; set; }
        public string CampaignID { get; set; }
        public string CouponID { get; set; }
        public string DeliveryType { get; set; }
        public bool IsFreeShipping { get; set; }
        public string DeliveryAddressID { get; set; }
        public string Platform { get; set; }
        public string PaymentTypeID { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
    }
}
