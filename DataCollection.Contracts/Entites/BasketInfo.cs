using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class BasketInfo
    {
        public string CurrentPrice { get; set; }
        public string OldPrice { get; set; }
        public bool InStock { get; set; }
        public bool IsQuickLook { get; set; }
        public int Quantity { get; set; }
    }
}
