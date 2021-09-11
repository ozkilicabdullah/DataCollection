using DataCollection.Contracts.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class SuccessFullCheckoutParams : ModelActivityBase
    {
        public string OrderID { get; set; }
        public int CampaignID { get; set; }
        public int CouponID { get; set; }
        public string DeliveryType { get; set; }
        public bool IsFreeShipping { get; set; }
        public string DeliveryAddressID { get; set; }
        public string Platform { get; set; }
        public int PaymentTypeID { get; set; }
        public ICollection<OrderedItem> OrderedItems { get; set; }
    }
}
