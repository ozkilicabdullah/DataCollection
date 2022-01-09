using DataCollection.Model;
using DataCollection.Model.Request;
using DataCollection.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCollection.Service.Middleware
{
    public class ApiActionFilterAttribute : TypeFilterAttribute
    {
        public ApiActionFilterAttribute() : base(typeof(ApiActionFilter))
        {

        }
    }
    public class ApiActionFilter : IActionFilter
    {
        private readonly IConnectionService ConnectionService;
        private readonly IConfiguration Configuration;

        public ApiActionFilter(IConnectionService connectionService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            Configuration = configuration;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            object actionRequest = null;
            if (!context.HttpContext.Items.TryGetValue("Request", out actionRequest))
                context.Result = new EmptyResult();

            var _actionRequest = context.HttpContext.Items["Request"] as ActionRequest;
            if (_actionRequest.Action == null)
            {
                context.Result = new EmptyResult();
                return;
            }
            _actionRequest.AppKey = _actionRequest.AppKey;

            var host = context.HttpContext.Request.Host.Value;

            User currentUser = ConnectionService.GetUserForClientId(_actionRequest.AppKey);
            bool isAllowDomain = false;
            var isAllow = false;

            string domainString = Configuration.GetValue<string>(currentUser.Username);
            if (!string.IsNullOrEmpty(domainString))
            {
                string[] domainList = domainString.Split(',');
                foreach (var item in domainList)
                {
                    if (host.Contains(item))
                        isAllowDomain = true;
                }
            }
            if (currentUser != null && isAllowDomain)
            {
                isAllow = true;
            }

            if (!isAllow)
                context.Result = new StatusCodeResult(403);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
