using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Contracts
{
    public class SearchParams : ModelActivityBase
    {
        public string Value { get; set; }
    }
    public class SearchPackage
    {
        public List<SearchParams> PackageSearch { get; set; }
    }
}
