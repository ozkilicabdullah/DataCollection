using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollection.Entities.Models
{
    public class Category
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ParentId { get; set; }
        public string Url { get; set; }
    }
}
