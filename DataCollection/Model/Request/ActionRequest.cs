using DataCollection.Contracts;
using System.Collections.Generic;

namespace DataCollection.Model.Request
{
    public class ActionRequest : ModelBase
    {
        public string Action { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }
}
