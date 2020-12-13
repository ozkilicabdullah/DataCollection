using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Model.Request.Hangfire
{
    public class RunHangfireTaskRequestParams: ModelBase
    {
        public string RouteUrl { get; set; }
        public string SecretKey { get; set; }
        public string TaskName { get; set; }
    }
}
