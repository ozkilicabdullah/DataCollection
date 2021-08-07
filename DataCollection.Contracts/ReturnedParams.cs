using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class ReturnedParams : ModelActivityBase
    {
        public int OrderID { get; set; }
        public bool PartialRefund { get; set; }
        public List<string> ProductID { get; set; }
    }
}
