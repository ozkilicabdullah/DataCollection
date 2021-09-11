using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class BasketInfo
    {
        public double CurrentPrice { get; set; }
        public double OldPrice { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public bool InStock { get; set; }
        public bool IsQuickLook { get; set; }
        public int Quantity { get; set; }
    }
}
