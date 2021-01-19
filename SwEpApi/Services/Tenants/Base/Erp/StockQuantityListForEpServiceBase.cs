using Dapper;
using SwEpApi.Entities.Base;
using SwEpApi.Helpers;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Services.Tenants.Base.Erp.RequestParams;
using SwEpApi.Validator.Erp;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.Erp
{
    public class StockQuantityListForEpServiceBase : ITenantService
    {
        private readonly IConnectionService ConnectionService;

        public StockQuantityListForEpServiceBase(IConnectionService connectionService)
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
            StockQuantityListForEpRequestParams Params = new StockQuantityListForEpRequestParams();
           Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as StockQuantityListForEpRequestParams;
            
            if(string.IsNullOrEmpty(Params.Barcode) && 
                string.IsNullOrEmpty(Params.ModelCode) && 
                string.IsNullOrEmpty(Params.ProductCode) && 
                (Params.StartDate==null || Params.EndDate==null))
            {
                response.Errors.Add("some parameters are missing");
            }
            response.Success = response.Errors.Count <= 0;

            if (!response.Success) return response;

            var dyp = new Dapper.DynamicParameters();
            dyp.Add("pageNo", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageNo);
            dyp.Add("pageSize", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageSize);
            dyp.Add("startDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.StartDate);
            dyp.Add("endDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.EndDate);
            dyp.Add("modelCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.ModelCode);
            dyp.Add("productCode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.ProductCode);
            dyp.Add("barcode", dbType: DbType.String, direction: ParameterDirection.Input, size: 40, value: Params.Barcode);

            var posts = await ConnectionService.ScopeAsync(Action.AppKey, cnn =>
           {
                return cnn.QueryAsync<EntityStockQuantityListForEpBase>("sp_SwEpApi_GetStockQuantityListForEp", 
                  param: dyp,                 
                  commandType: CommandType.StoredProcedure);
            });
            
            response.Data.Add("List", posts);

            return response;
        }
    }
}
