using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Service.Models
{
    public class RequestModel
    {
        public string AppKey { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }
}
