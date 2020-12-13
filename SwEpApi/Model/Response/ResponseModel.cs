using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Model.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
