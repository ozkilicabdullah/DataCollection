﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts.Entites
{
    public class OrderedItem
    {
        public string ProductID { get; set; }
        public bool IsFreeProduct { get; set; }
        public int Quantity { get; set; }
    }
}
