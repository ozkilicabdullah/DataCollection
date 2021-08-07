using DataCollection.Contracts;
using System.Collections.Generic;

namespace DataCollection.Model.Request
{

    public class ActionRequestModel : ModelBase
    {
        public List<ActionRequest> Action { get; set; }
    }

}
