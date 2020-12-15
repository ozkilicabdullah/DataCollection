using Serenity.Data;
using Serenity.Data.Mapping;
using System;

namespace SwEpApi.Entities.Base
{
    public abstract class EntityStockPriceBase : Row, IIdRow, INameRow
    {
        [Column("[ID]"),Identity]
        public Int64? Id
        {
            get { return EntityStockPriceBaseFields.Id[this]; }
            set { EntityStockPriceBaseFields.Id[this] = value; }
        }

        public string ModelCode
        {
            get { return EntityStockPriceBaseFields.ModelCode[this]; }
            set { EntityStockPriceBaseFields.ModelCode[this] = value; }
        }
        
        public string ProductCode
        {
            get { return EntityStockPriceBaseFields.ProductCode[this]; }
            set { EntityStockPriceBaseFields.ProductCode[this] = value; }
        }

        public string Barcode
        {
            get { return EntityStockPriceBaseFields.Barcode[this]; }
            set { EntityStockPriceBaseFields.Barcode[this] = value; }
        }

        public Decimal? ListPrice
        {
            get { return EntityStockPriceBaseFields.ListPrice[this]; }
            set { EntityStockPriceBaseFields.ListPrice[this] = value; }
        }
        public Decimal? SalePrice
        {
            get { return EntityStockPriceBaseFields.SalePrice[this]; }
            set { EntityStockPriceBaseFields.SalePrice[this] = value; }
        }
        public DateTime? CreatedDate
        {
            get { return EntityStockPriceBaseFields.CreatedDate[this]; }
            set { EntityStockPriceBaseFields.CreatedDate[this] = value; }
        }
        public DateTime? ModifiedDate
        {
            get { return EntityStockPriceBaseFields.ModifiedDate[this]; }
            set { EntityStockPriceBaseFields.ModifiedDate[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return EntityStockPriceBaseFields.Id; }
        }

        StringField INameRow.NameField
        {
            get { return EntityStockPriceBaseFields.Barcode; }
        }

        private EntityStockPriceBaseRowFields EntityStockPriceBaseFields;

        protected EntityStockPriceBase(RowFieldsBase fields)
                   : base(fields)
        {
            EntityStockPriceBaseFields = (EntityStockPriceBaseRowFields)fields;
        }

        public class EntityStockPriceBaseRowFields : RowFieldsBase
        {
            public Int64Field Id;
            public StringField ModelCode;
            public DecimalField ListPrice;
            public DecimalField SalePrice;
            public StringField ProductCode;
            public DateTimeField CreatedDate;
            public DateTimeField ModifiedDate;
            public StringField Barcode;
        }

    }
}
