using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class OrderedItem
    {
        public OrderedItem()
        {
            IsFreeProduct = false;
        }
        public string ProductID { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public bool IsFreeProduct { get; set; }
        public int Quantity { get; set; }
        public double CurrentPrice { get; set; }
        public double OldPrice { get; set; }
    }
}
