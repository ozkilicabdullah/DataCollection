using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class ViewParams : ModelActivityBase
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int ViewRange { get; set; }
    }
}
