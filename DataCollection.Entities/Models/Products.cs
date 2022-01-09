﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Entities.Models
{
    public class Products
    {
        public string ID { get; set; }
        public string Barcode { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string OldPrice { get; set; }
        public string Curreny { get; set; }
        public string Url { get; set; }
        // public Dictionary<int, string> Picture { get; set; } // angle(sorting),url
        public bool Available { get; set; }
        public string Categories { get; set; } // "1,2,56,796"
        public string Brand { get; set; }
        public string Departments { get; set; } // "1,3,5,6"
                                                // public Dictionary<string, string> Fashion { get; set; } //{gender:"male",sizes="X,M,L",colors="red,blue"}
        public List<Picture> Pictures { get; set; }
        public Fashion Fashions { get; set; }
    }

    public class Picture
    {
        public int Angle { get; set; }
        public string Url { get; set; }
    }
    public class Fashion
    {
        public string Gender { get; set; }
        public string Sizes { get; set; }
        public string Color { get; set; }
    }
}
