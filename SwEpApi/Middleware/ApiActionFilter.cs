using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SwEpApi.Model;
using SwEpApi.Model.Request;
using SwEpApi.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SwEpApi.Middleware
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

        public ApiActionFilter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.HasClaim(x => x.Type == "user"))
            {
                object actionRequest = null;
                object AppKey = "";

                if (!context.ActionArguments.TryGetValue("Request", out actionRequest) || !context.RouteData.Values.TryGetValue("app", out AppKey))
                    context.Result = new EmptyResult();

                var _actionRequest = context.ActionArguments["Request"] as ActionRequestModel;
                if (_actionRequest.Action.Count <= 0)
                {
                    context.Result = new EmptyResult();
                    return;
                }

                var Identity = context.HttpContext.User.Identity as ClaimsIdentity;
                var userName = (from c in Identity.Claims
                                where c.Type == "user"
                                select c.Value).FirstOrDefault();

                List<User> Users = new List<User>();
                Configuration.GetSection("Users").Bind(Users);

                var isAllow = false;
                var action = _actionRequest.Action[0].Action;
                var currentUser = (from c in Users where c.Username == userName select c).FirstOrDefault();
                if (currentUser != null)
                {
                    if (currentUser.Role == "superadmin") isAllow = true;
                    else if (currentUser.Perms!=null && currentUser.Perms.Count()>0)
                    {
                        if (currentUser.Perms.ContainsKey(Convert.ToString(AppKey)))
                            isAllow = (from c in currentUser.Perms[Convert.ToString(AppKey)] where c.Equals(Convert.ToString(action)) select c).Count() > 0;

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
