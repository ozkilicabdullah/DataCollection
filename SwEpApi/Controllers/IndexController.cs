using SwEpApi.Helpers;
using SwEpApi.Middleware;
using SwEpApi.Model.Request;
using SwEpApi.Model.Response;
using SwEpApi.Services;
using SwEpApi.Validator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SwEpApi.Controllers
{
    [Route("api/v1/[controller]/{app}")]
    [ApiController]
    [ApiActionFilter]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class RequestController : ControllerBase
    {     
        private readonly IRequestActionService Service;
        
        public RequestController(IRequestActionService _RequestActionService)
        {
            Service = _RequestActionService;
        }

        /// Service health
        /// </summary>
        [Route("service-status")]
        [HttpGet]
        //[Authorize()]
        public ActionResult<object> ServiceStatus()
        {
            var response = new ResponseModel()
            {
                Data = new Dictionary<string, object>(),
                Success = true,
                Errors = new List<string>()
            };

            return Ok(response);
        }
        
        /// <summary>
        /// Action Handler
        /// </summary>
        [Route("action")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseModel>> ActionRequest(string app, [FromBody] ActionRequestModel Request)
        {

            var response = new ResponseModel()
            {
                Data = new Dictionary<string, object>(),
                Success = true,
                Errors = new List<string>()
            };

            if (Request == null)
                throw new Exception("The request is invalid. cannot be empty.");           

            if (Request.Action == null)
                throw new Exception("The request is invalid. The 'Action' area cannot be empty.");

            if (Request.Action.Count <= 0)
                throw new Exception("The request is invalid. Action must have at least one collection.");

            response.Errors = Request.ValidateModel(new ActionRequestModelValidator());
            response.Success = response.Errors.Count <= 0;
            var RequestAction = Request.Action[0];

            if (response.Success)
            {
                RequestAction.AppKey = app;
                response = await Service.Request(RequestAction);
            }

            return Ok(response);
        } 

    }
}




