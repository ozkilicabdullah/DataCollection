

using System;

namespace DataCollection.Contracts
{
    public class ModelBase
    {
        public string AppKey { get; set; }
    }
    public class ModelActivityBase : ModelBase
    {
        public string UserID { get; set; }
        public string SessionID { get; set; }
    }
}
