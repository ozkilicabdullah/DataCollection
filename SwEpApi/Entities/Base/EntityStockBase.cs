using Serenity.Data;
using Serenity.Data.Mapping;
using System;

namespace SwEpApi.Entities.Base
{
    public abstract class EntityStockBase : Row, IIdRow, INameRow
    {
        [Column("[ID]"),Identity]
        public Int64? Id
        {
            get { return EntityStockBaseFields.Id[this]; }
            set { EntityStockBaseFields.Id[this] = value; }
        }

        public string ModelCode
        {
            get { return EntityStockBaseFields.ModelCode[this]; }
            set { EntityStockBaseFields.ModelCode[this] = value; }
        }
        
        public string ProductCode
        {
            get { return EntityStockBaseFields.ProductCode[this]; }
            set { EntityStockBaseFields.ProductCode[this] = value; }
        }

        public string Barcode
        {
            get { return EntityStockBaseFields.Barcode[this]; }
            set { EntityStockBaseFields.Barcode[this] = value; }
        }

        public string ProductName
        {
            get { return EntityStockBaseFields.ProductName[this]; }
            set { EntityStockBaseFields.ProductName[this] = value; }
        }

        public string ProductDesc
        {
            get { return EntityStockBaseFields.ProductDesc[this]; }
            set { EntityStockBaseFields.ProductDesc[this] = value; }
        }
        public Int32? Quantity
        {
            get { return EntityStockBaseFields.Quantity[this]; }
            set { EntityStockBaseFields.Quantity[this] = value; }
        }
        public Decimal? ListPrice
        {
            get { return EntityStockBaseFields.ListPrice[this]; }
            set { EntityStockBaseFields.ListPrice[this] = value; }
        }
        public Decimal? SalePrice
        {
            get { return EntityStockBaseFields.SalePrice[this]; }
            set { EntityStockBaseFields.SalePrice[this] = value; }
        }
        public DateTime? CreatedDate
        {
            get { return EntityStockBaseFields.CreatedDate[this]; }
            set { EntityStockBaseFields.CreatedDate[this] = value; }
        }
        public DateTime? ModifiedDate
        {
            get { return EntityStockBaseFields.ModifiedDate[this]; }
            set { EntityStockBaseFields.ModifiedDate[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return EntityStockBaseFields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return EntityStockBaseFields.ProductName; }
        }

        private EntityStockBaseRowFields EntityStockBaseFields;

        protected EntityStockBase(RowFieldsBase fields)
                   : base(fields)
        {
            EntityStockBaseFields = (EntityStockBaseRowFields)fields;
        }

        public class EntityStockBaseRowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField ModelCode;
            public Int32Field Quantity;
            public DecimalField ListPrice;
            public DecimalField SalePrice;
            public StringField ProductCode;
            public DateTimeField CreatedDate;
            public DateTimeField ModifiedDate;
            public StringField Barcode;
            public StringField ProductName;
            public StringField ProductDesc;
        }

    }
}
