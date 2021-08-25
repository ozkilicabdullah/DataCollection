using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Entities.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string CampaignId { get; set; }
        public string CouponId { get; set; }
        public string PaymentId { get; set; }
        public string DeliveryType { get; set; }
        public string DeliveryAddressId { get; set; }
        public string Platform { get; set; }
        public List<OrderedItems> OrderedItems { get; set; }
    }

    public class OrderedItems
    {
        public string ProductId { get; set; }
        public string IsFreeProduct { get; set; }
        public string Quantity { get; set; }
    }
}
