

using System;

namespace DataCollection.Contracts
{
    public class ModelBase
    {
        public string AppKey { get; set; }
    }
    public class ModelActivityBase : ModelBase
    {
        public ModelActivityBase()
        {
            CreatedOn = DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss");
        }
        public string UserID { get; set; }
        public string SessionID { get; set; }
        public string CreatedOn { get; set; }
    }
}
