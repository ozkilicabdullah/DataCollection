using DataCollection.Contracts.Entites.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class ReturnedProduct
    {
        public string ProductID { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public ReturnReason ReturnReason { get; set; }
    }
}
