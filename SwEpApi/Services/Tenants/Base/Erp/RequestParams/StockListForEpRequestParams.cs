using SwEpApi.Model;
using System;
using static SwEpApi.Helpers.Enums;

namespace SwEpApi.Services.Tenants.Base.Order
{
    public class StockListForEpRequestParams : ModelListBase
    {
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}
