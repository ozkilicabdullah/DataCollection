using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SwEpApi.Entities.Base
{
    public class EntityStockDetailBase
    {
        public EntityStockDetailBase()
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
        public string ProductSizeTableData { get; set; }
        public string SeasonName { get; set; }
        public string ManikinInformations { get; set; }
        public string DepartmentName { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnore]
        public string ProductImagesJson { get; set; }
        public List<EntityImages> ProductImages { get; set; }
    }
}