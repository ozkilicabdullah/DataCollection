using Serenity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwEpApi.Entities.Base
{
    public class EntityStockListForEpBase : EntityStockBase
    {

        public static EntityStockListForEpBaseRowFields EntityStockListForEpFields = new EntityStockListForEpBaseRowFields().Init();

        public EntityStockListForEpBase()
            : base(EntityStockListForEpFields)
        {

        }

        public class EntityStockListForEpBaseRowFields : EntityStockBaseRowFields
        {
        }
    }
}
