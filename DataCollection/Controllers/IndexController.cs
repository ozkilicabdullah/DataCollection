using DataCollection.Helpers;
using DataCollection.Middleware;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Services;
using DataCollection.Validator;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace DataCollection.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ApiActionFilter]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class RequestController : ControllerBase
    {
        private readonly IRequestActionService Service;
        private readonly IConnectionService connectionService;

        public RequestController(IRequestActionService _RequestActionService, IConnectionService _connectionService)
        {
            Service = _RequestActionService;
            connectionService = _connectionService;
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
                Success = true,
                Errors = new List<string>()
            };

            return Ok(response);
        }

        /// <summary>
        /// Action Handler
        /// </summary>
        //[Route("action")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseModel>> ActionRequest([FromBody] ActionRequestModel Request)
        {

            var response = new ResponseModel()
            {
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

            bool isExist = connectionService.GetTenant(Request.AppKey);
            if (!isExist)
                throw new Exception("The request is invalid.Url is not correct.");


            if (response.Success)
            {
                RequestAction.AppKey = Request.AppKey;
                response = await Service.Request(RequestAction);
            }

            return Ok(response);
        }

    }
}




