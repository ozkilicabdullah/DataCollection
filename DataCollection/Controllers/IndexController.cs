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
        public async Task<ActionResult<ResponseModel>> ActionRequest([FromBody] ActionRequest Request)
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

            if (Request == null)
                throw new Exception("The request is invalid. Action must have at least one collection.");

            response.Errors = Request.ValidateModel(new ActionValidator());
            response.Success = response.Errors.Count <= 0;

            bool isExist = connectionService.GetTenant(Request.AppKey);
            if (!isExist)
            {
                response.Success = false;
                response.Errors.Add("'AppKey' is not correct.");
                return Unauthorized(response);
            }

            if (response.Success)
            {
                Request.AppKey = Request.AppKey;
                response = await Service.Request(Request);
            }

            return Ok(response);
        }

    }
}




