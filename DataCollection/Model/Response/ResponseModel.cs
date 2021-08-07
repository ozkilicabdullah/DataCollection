using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Model.Response
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
    public class LoginReponseModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public Dictionary<string, object> User { get; set; }

    }
}
