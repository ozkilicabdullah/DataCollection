using Dapper;
using SwEpApi.Entities.Base;
using SwEpApi.Helpers;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.Order
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

            response.Success = response.Errors.Count <= 0;

            if (!response.Success) return response;

            var dyp = new Dapper.DynamicParameters();
            dyp.Add("pageNo", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageNo);
            dyp.Add("pageSize", dbType: DbType.Int32, direction: ParameterDirection.Input, value: Params.pageSize);
            dyp.Add("orderbyField", dbType: DbType.String, direction: ParameterDirection.Input,size:30, value: Params.orderbyField);
            dyp.Add("orderbyDesc", dbType: DbType.Boolean, direction: ParameterDirection.Input, value: Params.orderbyDesc);
            dyp.Add("startDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.StartDate);
            dyp.Add("endDate", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.EndDate);

            var posts = await ConnectionService.ScopeAsync(Action.AppKey, cnn =>
           {
                return cnn.QueryAsync<EntityStockListForEpBase>("sp_SwEpApi_GetStockListForEp", 
                  param: dyp,                 
                  commandType: CommandType.StoredProcedure);
            });
            
            response.Data.Add("List", posts);

            return response;
        }
    }
}
