using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Entities.Models
{
    public class Address
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Detail { get; set; }
    }
}
