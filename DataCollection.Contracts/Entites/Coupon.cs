using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Entities.Base
{
    public class Coupon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMultiple { get; set; }
        public SaleType SaleType { get; set; }
        public string ExpiryDate { get; set; }

    }
}
