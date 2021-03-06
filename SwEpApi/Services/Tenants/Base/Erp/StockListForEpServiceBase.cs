﻿using Dapper;
using Newtonsoft.Json;
using SwEpApi.Entities.Base;
using SwEpApi.Helpers;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Validator.Erp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.Erp
{
    public class StockListForEpServiceBase : ITenantService
    {
        private readonly IConnectionService ConnectionService;

        public StockListForEpServiceBase(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }

        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Data = new Dictionary<string, object>(),
                Errors = new List<string>()
            };

            ActionRequest Action = Payload["Action"] as ActionRequest;
            StockListForEpRequestParams Params = new StockListForEpRequestParams();
            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as StockListForEpRequestParams;
            response.Errors = Params.ValidateModel(new StockListForEpValidator());
            if (string.IsNullOrEmpty(Params.Barcode) &&
                string.IsNullOrEmpty(Params.ModelCode) &&
                string.IsNullOrEmpty(Params.ProductCode) &&
                (Params.StartDate == null || Params.EndDate == null))
            {
                response.Errors.Add("some parameters are missing");
            }
            response.Success = response.Errors.Count <= 0;

            if (!response.Success) return response;

            var dyp = new Dapper.DynamicParameters();
            dyp.Add("pageNo", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageNo);
            dyp.Add("pageSize", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageSize);
            dyp.Add("langCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 2, value: Params.LangCode);
            dyp.Add("priceCurrencyCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 3, value: Params.PriceCurrencyCode);
            dyp.Add("startDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.StartDate);
            dyp.Add("endDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.EndDate);
            dyp.Add("modelCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.ModelCode);
            dyp.Add("productCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.ProductCode);
            dyp.Add("barcode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.Barcode);
            dyp.Add("isActive", dbType: DbType.Boolean, direction: ParameterDirection.Input, value: Params.IsActive);

            var posts = await ConnectionService.ScopeAsync(Action.AppKey, cnn =>
           {
               return cnn.QueryAsync<EntityStockListForEpBase>("sp_SwEpApi_GetStockListForEp",
                 param: dyp,
                 commandType: CommandType.StoredProcedure);
           });
            try
            {
                if (posts != null)
                {
                    foreach (EntityStockListForEpBase cust in posts)
                    {
                        List<object> Attributes = new List<object>();
                        if (!string.IsNullOrEmpty(cust.AttributesJson))
                        {
                            List<Dictionary<string, string>> _attrList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(cust.AttributesJson);
                            if(_attrList != null)
                            {
                                foreach(Dictionary<string, string> _att1 in _attrList)
                                {
                                    foreach (string _attkey in _att1.Keys)
                                    {
                                        Attributes.Add(new { AttributeName = _attkey, AttributeValue = _att1[_attkey] });
                                    }
                                }
                            }
                        }
                        else
                        {
                            Attributes.Add(new { AttributeName = "Sezon", AttributeValue = cust.SeasonName });
                            Attributes.Add(new { AttributeName = "Renk", AttributeValue = cust.ColorName });
                            Attributes.Add(new { AttributeName = "Departman", AttributeValue = cust.DepartmentName });
                            Attributes.Add(new { AttributeName = "Marka", AttributeValue = cust.BrandName });
                            Attributes.Add(new { AttributeName = "Size", AttributeValue = cust.SizeName });
                        }
                        cust.AttributesJson = null;
                        cust.Attributes = Attributes;

                        if (!string.IsNullOrEmpty(cust.ProductImagesJson))
                        {
                            cust.ProductImages = JsonConvert.DeserializeObject<List<EntityImages>>(cust.ProductImagesJson);
                        }
                        else
                        {
                            cust.ProductImages = null;
                        }
                        cust.ProductImagesJson = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }


            response.Data.Add("List", posts);

            return response;
        }
    }
}
