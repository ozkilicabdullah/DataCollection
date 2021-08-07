using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Entities.Base
{
    public class Campaign
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public SaleType SaleType { get; set; }
        public string ExpiryDate { get; set; }
    }

    public enum SaleType
    {
        Cash = 1,
        Percent = 2
    }
}
