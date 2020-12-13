using Microsoft.AspNetCore.Http;
using SwEpApi.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwEpApi.Services.Tenants
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITenantService
    {
        Task<ResponseModel> Execute(Dictionary<string,object> Payload, string Identifer);
    }

}
