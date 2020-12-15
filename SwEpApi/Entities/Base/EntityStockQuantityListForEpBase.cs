using Serenity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Entities.Base
{
    public class EntityStockQuantityListForEpBase : EntityStockQuantityBase
    {
        public int TotalCount
        {
            get { return (int)EntityStockQuantityListForEpFields.TotalCount[this]; }
            set { EntityStockQuantityListForEpFields.TotalCount[this] = (int)value; }
        }
        public static EntityStockQuantityListForEpBaseRowFields EntityStockQuantityListForEpFields = new EntityStockQuantityListForEpBaseRowFields().Init();

        public EntityStockQuantityListForEpBase()
            : base(EntityStockQuantityListForEpFields)
        {

        }

        public class EntityStockQuantityListForEpBaseRowFields : EntityStockQuantityBaseRowFields
        {
            public Int32Field TotalCount;
        }
    }
}
