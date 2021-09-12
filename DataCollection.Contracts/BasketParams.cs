using DataCollection.Contracts.Entites;
using System.Collections.Generic;

namespace DataCollection.Contracts
{
    public class BasketParams : ModelActivityBase
    {
        public string ProductID { get; set; }
        public BasketInfo BasketInfo { get; set; }
        public string Type { get; set; }
    }
    public class BasketPackage
    {
        public List<BasketParams> package { get; set; }
    }
}
