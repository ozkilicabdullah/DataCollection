using DataCollection.Contracts.Entites;
using DataCollection.Contracts.Entites.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class ReturnedParams : ModelActivityBase
    {
        public ReturnedParams()
        {
            ReturnReason = ReturnReason.RightOfWithdrawal;
            PartialReturn = false;
        }
        public int OrderID { get; set; }
        public bool PartialReturn { get; set; }
        public ReturnReason ReturnReason { get; set; }
        //public List<string> ProductID { get; set; }
        public List<ReturnedProduct> ReturnedProduct { get; set; }
    }
}
