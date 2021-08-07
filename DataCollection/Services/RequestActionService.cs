using Microsoft.Extensions.Configuration;
using DataCollection.Providers;
using DataCollection.Services.Tenants;
using Microsoft.Extensions.Logging;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace DataCollection.Services
{
    public class RequestActionService : IRequestActionService
    {
        private readonly IServicesProvider<ITenantService> TenantServiceProvider;

        public RequestActionService(IServicesProvider<ITenantService> tenantServiceProvider)
        {
            TenantServiceProvider = tenantServiceProvider;

        }

        public async Task<ResponseModel> Request(ActionRequest Action)
        {
            var ActionServiceName = Convert.ToString(string.Format("{0}{1}", Action.Action, "Producer"));
            ITenantService actionService = TenantServiceProvider.GetInstance(ActionServiceName);
            Action.Payload.Add("AppKey", Action.AppKey);
            var response = await actionService.Execute(new Dictionary<string, object>() { { "Action", Action }, { "Payload", Action.Payload } }, ActionServiceName);

            return response;

        }
    }
}
