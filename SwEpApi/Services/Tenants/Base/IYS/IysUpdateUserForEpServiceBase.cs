using Dapper;
using SwEpApi.Helpers;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Services.Tenants.Base.IYS.RequestParams;
using SwEpApi.Validator.IYS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants.Base.IYS
{
    public class IysUpdateUserForEpServiceBase : ITenantService
    {
        private readonly IConnectionService ConnectionService;

        public IysUpdateUserForEpServiceBase(IConnectionService connectionService)
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
            IYSUpdateRequestParams Params = new IYSUpdateRequestParams();
            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as IYSUpdateRequestParams;

            response.Errors = Params.ValidateModel(new IYSUpdateValidator());
            response.Success = response.Errors.Count <= 0;

            if (!response.Success) return response;

            bool isAllow = Params.status == "ONAY" ? true : false;
            int allowType = Params.type == "EPOSTA" ? 0 : 1;// 0 -> E-Posta 1 -> Sms ,Arama

            var dyp = new Dapper.DynamicParameters();

            dyp.Add("ModifiedOn", dbType: DbType.DateTime, direction: ParameterDirection.Input, value: Params.creationDate);
            dyp.Add("allowType", dbType: DbType.Int64, direction: ParameterDirection.Input, value: allowType);
            dyp.Add("isAllow", dbType: DbType.Boolean, direction: ParameterDirection.Input, value: isAllow);
            dyp.Add("recipient", dbType: DbType.String, direction: ParameterDirection.Input, value: Params.recipient);
            dyp.Add("returnValue", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int posts = await ConnectionService.ScopeAsync(Action.AppKey, cnn =>
            {
                return cnn.QueryFirstOrDefaultAsync<int>("sp_SwEpApi_UpdateUserForIYS",
                  param: dyp,
                  commandType: CommandType.StoredProcedure);
            });
            int returnValue = dyp.Get<int>("returnValue");
            try
            {
                if (returnValue > 0)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Errors.Add("User not found.");
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }

    }
}
