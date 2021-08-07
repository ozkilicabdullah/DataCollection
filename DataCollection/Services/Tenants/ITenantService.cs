using Microsoft.AspNetCore.Http;
using DataCollection.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITenantService
    {
        Task<ResponseModel> Execute(Dictionary<string,object> Payload, string Identifer);
    }

}
