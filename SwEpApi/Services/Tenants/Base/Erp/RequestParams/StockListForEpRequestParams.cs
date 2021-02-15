using SwEpApi.Model;
using System;
using static SwEpApi.Helpers.Enums;

namespace SwEpApi.Services.Tenants.Base.Erp
{
    public class StockListForEpRequestParams : ModelListBase
    {
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public Nullable<Boolean> IsActive { get; set; }
        public string LangCode { get; set; }
        public string PriceCurrencyCode { get; set; }
        public string ModelCode { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
    }
}
