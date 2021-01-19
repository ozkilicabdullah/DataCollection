using SwEpApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.Erp.RequestParams
{
    public class StockQuantityListForEpRequestParams : ModelListBase
    {
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string ModelCode { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
    }
}
