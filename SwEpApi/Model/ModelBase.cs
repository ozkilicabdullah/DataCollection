using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Model
{
    public class ModelBase
    {
        [Required(ErrorMessage = "AppKey is Required")]
        public string AppKey { get; set; }
    }

    public class ModelListBase: ModelBase
    {
        public ModelListBase()
        {
            this.pageNo = 1;
            this.pageSize = 1;
            this.orderbyDesc = false;
        }
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public string orderbyField { get; set; }
        public bool orderbyDesc { get; set; } //-- 0 ise asc, 1 ise desc
    }
}
