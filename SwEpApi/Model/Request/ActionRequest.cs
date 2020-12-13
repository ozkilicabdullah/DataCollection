using SwEpApi.Services.Tenants.Base;
using System.Collections.Generic;

namespace SwEpApi.Model.Request
{
    public class ActionRequest : ModelBase
    {
        public string Action { get; set; }
        public Dictionary<string, object> Payload { get; set; }
    }
}
