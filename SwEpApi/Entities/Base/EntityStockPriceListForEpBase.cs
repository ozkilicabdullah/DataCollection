using Serenity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Entities.Base
{
    public class EntityStockPriceListForEpBase : EntityStockPriceBase
    {
        public int TotalCount
        {
            get { return (int)EntityStockPriceListForEpFields.TotalCount[this]; }
            set { EntityStockPriceListForEpFields.TotalCount[this] = (int)value; }
        }
        public static EntityStockPriceListForEpBaseRowFields EntityStockPriceListForEpFields = new EntityStockPriceListForEpBaseRowFields().Init();

        public EntityStockPriceListForEpBase()
            : base(EntityStockPriceListForEpFields)
        {

        }

        public class EntityStockPriceListForEpBaseRowFields : EntityStockPriceBaseRowFields
        {
            public Int32Field TotalCount;
        }
    }
}
