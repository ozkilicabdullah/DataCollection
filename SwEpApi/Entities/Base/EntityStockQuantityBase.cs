using Serenity.Data;
using Serenity.Data.Mapping;
using System;

namespace SwEpApi.Entities.Base
{
    public abstract class EntityStockQuantityBase : Row, IIdRow, INameRow
    {
        [Column("[ID]"),Identity]
        public Int64? Id
        {
            get { return EntityStockQuantityBaseFields.Id[this]; }
            set { EntityStockQuantityBaseFields.Id[this] = value; }
        }

        public string ModelCode
        {
            get { return EntityStockQuantityBaseFields.ModelCode[this]; }
            set { EntityStockQuantityBaseFields.ModelCode[this] = value; }
        }
        
        public string ProductCode
        {
            get { return EntityStockQuantityBaseFields.ProductCode[this]; }
            set { EntityStockQuantityBaseFields.ProductCode[this] = value; }
        }

        public string Barcode
        {
            get { return EntityStockQuantityBaseFields.Barcode[this]; }
            set { EntityStockQuantityBaseFields.Barcode[this] = value; }
        }

        public Int32? Quantity
        {
            get { return EntityStockQuantityBaseFields.Quantity[this]; }
            set { EntityStockQuantityBaseFields.Quantity[this] = value; }
        }
        public DateTime? CreatedDate
        {
            get { return EntityStockQuantityBaseFields.CreatedDate[this]; }
            set { EntityStockQuantityBaseFields.CreatedDate[this] = value; }
        }
        public DateTime? ModifiedDate
        {
            get { return EntityStockQuantityBaseFields.ModifiedDate[this]; }
            set { EntityStockQuantityBaseFields.ModifiedDate[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return EntityStockQuantityBaseFields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return EntityStockQuantityBaseFields.Barcode; }
        }

        private EntityStockQuantityBaseRowFields EntityStockQuantityBaseFields;

        protected EntityStockQuantityBase(RowFieldsBase fields)
                   : base(fields)
        {
            EntityStockQuantityBaseFields = (EntityStockQuantityBaseRowFields)fields;
        }

        public class EntityStockQuantityBaseRowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField ModelCode;
            public Int32Field Quantity;
            public StringField ProductCode;
            public DateTimeField CreatedDate;
            public DateTimeField ModifiedDate;
            public StringField Barcode;
        }

    }
}
