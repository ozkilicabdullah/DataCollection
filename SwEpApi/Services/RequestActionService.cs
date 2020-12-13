using Microsoft.Extensions.Configuration;
using SwEpApi.Providers;
using SwEpApi.Services.Tenants;
using Microsoft.Extensions.Logging;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SwEpApi.Services
{
    public class RequestActionService : IRequestActionService
    {
        private readonly IConfiguration Configuration;
        private readonly IConnectionService ConnectionService;
        private readonly IServicesProvider<ITenantService> TenantServiceProvider;
        private readonly ILogger<IRequestActionService> Logger;

        public RequestActionService(IConfiguration configuration, 
            IConnectionService connectionService, 
            ILogger<IRequestActionService> logger,
            IServicesProvider<ITenantService> tenantServiceProvider)
        {
            Configuration = configuration;
            ConnectionService = connectionService;
            TenantServiceProvider = tenantServiceProvider;
            Logger = logger;
        }

        public async Task<ResponseModel> Request(ActionRequest Action)
        {
            var ActionServiceName = Convert.ToString(string.Format("{0}{1}{2}", Action.Action, "Service", Action.AppKey));
            ITenantService actionService = TenantServiceProvider.GetInstance(ActionServiceName);
            Action.Payload.Add("AppKey", Action.AppKey);
            var response = await actionService.Execute(new Dictionary<string, object>() { { "Action", Action }, { "Payload", Action.Payload } }, ActionServiceName);

            return response;

        }
    }
}
