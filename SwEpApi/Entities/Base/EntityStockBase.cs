using Newtonsoft.Json;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.Collections.Generic;

namespace SwEpApi.Entities.Base
{

    public class EntityStockBase
    {
        public EntityStockBase()
        {
            this.ProductImages = new List<EntityImages>();
        }
        public long Id { get; set; }
        public string ModelCode { get; set; }
        public int Quantity { get; set; }
        public Nullable<Decimal> ListPrice { get; set; }
        public Nullable<Decimal> SalePrice { get; set; }
        public string ProductCode { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductUrl { get; set; }
        public string SeasonName { get; set; }
        public string DepartmentName { get; set; }
        public string GoogleCategoryCode { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public string ProductImagesJson { get; set; }
        public List<EntityImages> ProductImages { get; set; }
    }
    public class EntityImages
    {
        public string Url { get; set; }
    }

    //    public abstract class EntityStockBase : Row, IIdRow, INameRow
    //    {
    //        [Column("[ID]"),Identity]
    //        public Int64? Id
    //        {
    //            get { return EntityStockBaseFields.Id[this]; }
    //            set { EntityStockBaseFields.Id[this] = value; }
    //        }

    //        public string ModelCode
    //        {
    //            get { return EntityStockBaseFields.ModelCode[this]; }
    //            set { EntityStockBaseFields.ModelCode[this] = value; }
    //        }

    //        public string ProductCode
    //        {
    //            get { return EntityStockBaseFields.ProductCode[this]; }
    //            set { EntityStockBaseFields.ProductCode[this] = value; }
    //        }

    //        public string Barcode
    //        {
    //            get { return EntityStockBaseFields.Barcode[this]; }
    //            set { EntityStockBaseFields.Barcode[this] = value; }
    //        }

    //        public string ProductName
    //        {
    //            get { return EntityStockBaseFields.ProductName[this]; }
    //            set { EntityStockBaseFields.ProductName[this] = value; }
    //        }

    //        public string ProductDesc
    //        {
    //            get { return EntityStockBaseFields.ProductDesc[this]; }
    //            set { EntityStockBaseFields.ProductDesc[this] = value; }
    //        }
    //        public string SeasonName
    //        {
    //            get { return EntityStockBaseFields.SeasonName[this]; }
    //            set { EntityStockBaseFields.SeasonName[this] = value; }
    //        }
    //        public string DepartmentName
    //        {
    //            get { return EntityStockBaseFields.DepartmentName[this]; }
    //            set { EntityStockBaseFields.DepartmentName[this] = value; }
    //        }
    //        public string BrandName
    //        {
    //            get { return EntityStockBaseFields.BrandName[this]; }
    //            set { EntityStockBaseFields.BrandName[this] = value; }
    //        }
    //        public string ColorName
    //        {
    //            get { return EntityStockBaseFields.ColorName[this]; }
    //            set { EntityStockBaseFields.ColorName[this] = value; }
    //        }
    //        public string SizeName
    //        {
    //            get { return EntityStockBaseFields.SizeName[this]; }
    //            set { EntityStockBaseFields.SizeName[this] = value; }
    //        }
    //        public string ProductImages
    //        {
    //            get { return EntityStockBaseFields.ProductImages[this]; }
    //            set { EntityStockBaseFields.ProductImages[this] = value; }
    //        }
    //        public Int32? Quantity
    //        {
    //            get { return EntityStockBaseFields.Quantity[this]; }
    //            set { EntityStockBaseFields.Quantity[this] = value; }
    //        }
    //        public Decimal? ListPrice
    //        {
    //            get { return EntityStockBaseFields.ListPrice[this]; }
    //            set { EntityStockBaseFields.ListPrice[this] = value; }
    //        }
    //        public Decimal? SalePrice
    //        {
    //            get { return EntityStockBaseFields.SalePrice[this]; }
    //            set { EntityStockBaseFields.SalePrice[this] = value; }
    //        }
    //        public DateTime? CreatedDate
    //        {
    //            get { return EntityStockBaseFields.CreatedDate[this]; }
    //            set { EntityStockBaseFields.CreatedDate[this] = value; }
    //        }
    //        public DateTime? ModifiedDate
    //        {
    //            get { return EntityStockBaseFields.ModifiedDate[this]; }
    //            set { EntityStockBaseFields.ModifiedDate[this] = value; }
    //        }
    //        public bool? IsActive
    //        {
    //            get { return EntityStockBaseFields.IsActive[this]; }
    //            set { EntityStockBaseFields.IsActive[this] = value; }
    //        }

    //        IIdField IIdRow.IdField
    //        {
    //            get { return EntityStockBaseFields.Id; }
    //        }

    //        StringField INameRow.NameField
    //        {
    //            get { return EntityStockBaseFields.ProductName; }
    //        }

    //        private EntityStockBaseRowFields EntityStockBaseFields;

    //        protected EntityStockBase(RowFieldsBase fields)
    //                   : base(fields)
    //        {
    //            EntityStockBaseFields = (EntityStockBaseRowFields)fields;
    //        }

    //        public class EntityStockBaseRowFields : RowFieldsBase
    //        {
    //            public Int64Field Id;
    //            public StringField ModelCode;
    //            public Int32Field Quantity;
    //            public DecimalField ListPrice;
    //            public DecimalField SalePrice;
    //            public StringField ProductCode;
    //            public DateTimeField CreatedDate;
    //            public DateTimeField ModifiedDate;
    //            public StringField Barcode;
    //            public StringField ProductName;
    //            public StringField ProductDesc;
    //            public StringField SeasonName;
    //            public StringField DepartmentName;
    //            public StringField BrandName;
    //            public StringField ColorName;
    //            public StringField SizeName;
    //            public StringField ProductImages;
    //            public BooleanField IsActive;
    //        }

    //    }
}
