using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Entities.Base
{
    public class Address
    {
        public Address()
        {
            Type = "Individual";
        }
        public int ID { get; set; }
        public string UserID { get; set; }
        public string SessionID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Detail { get; set; }
        public string Type { get; set; } // Individual - Corporate
    }

}
