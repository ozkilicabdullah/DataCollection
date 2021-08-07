using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class WishInfo
    {
        public string CurrentPrice { get; set; }
        public string OldPrice { get; set; }
        public bool InStock { get; set; }
        public bool IsQuickLook { get; set; }
    }
}
