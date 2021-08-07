

using System;

namespace DataCollection.Contracts
{
    public class ModelBase
    {
        public string AppKey { get; set; }
    }
    public class ModelActivityBase : ModelBase
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
    }
}
