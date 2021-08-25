using DataCollection.Contracts;
using System.Collections.Generic;

namespace DataCollection.Model.Request
{
    public class LoginRequest : ModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
    }
}
