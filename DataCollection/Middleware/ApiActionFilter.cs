using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DataCollection.Model;
using DataCollection.Model.Request;
using DataCollection.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using DataCollection.Services;

namespace DataCollection.Middleware
{

    public class ApiActionFilterAttribute : TypeFilterAttribute
    {
        public ApiActionFilterAttribute() : base(typeof(ApiActionFilter))
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiActionFilter : IActionFilter
    {
        private readonly IConfiguration Configuration;
        private readonly IConnectionService ConnectionService;

        public ApiActionFilter(IConfiguration configuration, IConnectionService connectionService)
        {
            Configuration = configuration;
            ConnectionService = connectionService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.HasClaim(x => x.Type == "user"))
            {
                object actionRequest = null;
                //object AppKey = "";
                if (!context.ActionArguments.TryGetValue("Request", out actionRequest))
                    context.Result = new EmptyResult();


                var _actionRequest = context.ActionArguments["Request"] as ActionRequest;
                if (_actionRequest.Action == null)
                {
                    context.Result = new EmptyResult();
                    return;
                }
                _actionRequest.AppKey = _actionRequest.AppKey;
                var Identity = context.HttpContext.User.Identity as ClaimsIdentity;
                var userName = (from c in Identity.Claims
                                where c.Type == "user"
                                select c.Value).FirstOrDefault();
                var password = (from c in Identity.Claims
                                where c.Type == "password"
                                select c.Value).FirstOrDefault();

                User currentUser = ConnectionService.GetCurrentUser(userName, password);

                var isAllow = false;
                var action = _actionRequest.Action;
                if (currentUser != null)
                {
                    if (currentUser.Role == "superadmin") isAllow = true;
                    else if (currentUser.Perms != null && currentUser.Perms.Count() > 0)
                    {
                        if (currentUser.Perms.Contains(Convert.ToString(action)))
                            isAllow = true;
                    }

                }

                if (!isAllow)
                    context.Result = new StatusCodeResult(403);

            }
            else
            {
                context.Result = new StatusCodeResult(403);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

    }
}
