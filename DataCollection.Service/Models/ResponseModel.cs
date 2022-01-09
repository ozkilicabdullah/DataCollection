using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Service.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
