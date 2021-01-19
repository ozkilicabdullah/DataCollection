using Dapper;
using Newtonsoft.Json;
using SwEpApi.Entities.Base;
using SwEpApi.Helpers;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Services.Tenants.Base.Erp.RequestParams;
using SwEpApi.Validator.Erp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.Erp
{
    public class StockDetailListForEpServiceBase : ITenantService
    {
        private readonly IConnectionService ConnectionService;

        public StockDetailListForEpServiceBase(IConnectionService connectionService)
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

            StockDetailListForEpRequestParams Params = new StockDetailListForEpRequestParams();

            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as StockDetailListForEpRequestParams;

            response.Errors = Params.ValidateModel(new StockDetailListForEpValidator());

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

            var posts = await ConnectionService.ScopeAsync(Action.AppKey, cnn =>
           {
               return cnn.QueryAsync<EntityStockDetailListForEpBase>("sp_SwEpApi_GetStockDetailListForEp",
                 param: dyp,
                 commandType: CommandType.StoredProcedure);
           });
            try
            {
                if (posts != null)
                {
                    foreach (EntityStockDetailListForEpBase cust in posts)
                    {
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
