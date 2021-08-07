using DataCollection.Contracts.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class WishParams : ModelActivityBase
    {
        public string ProductID { get; set; }
        public string Type { get; set; }
        public WishInfo WishInfo { get; set; }
    }
}
