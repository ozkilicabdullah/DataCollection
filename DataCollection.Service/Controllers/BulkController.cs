using DataCollection.Entities.Models;
using DataCollection.Helpers;
using DataCollection.Service.Middleware;
using DataCollection.Service.Models;
using DataCollection.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DataCollection.Service.Controllers
{
    [Route("api/v1/[controller]/")]
  //  [ApiActionFilter]
    [ApiController]
    public class BulkController : ControllerBase
    {
        private readonly IConnectionService ConnectionService;
        public BulkController(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        // POST api/values
        [HttpPost]
        [Route("Products")]
        public ActionResult<ResponseModel> ProductsInsert([FromBody] RequestModel Request)
        {
            var response = new ResponseModel()
            {
                Success = true,
                Errors = new List<string>()
            };

            if (Request == null)
                throw new Exception("The request is invalid. cannot be empty.");

            if (Request.AppKey == null)
                throw new Exception("The request is invalid. The 'AppKey' area cannot be empty.");

            if (Request.Payload == null)
                throw new Exception("The request is invalid. The 'Payload' area cannot be empty.");


            bool isExist = ConnectionService.GetTenant(Request.AppKey);
            if (!isExist)
            {
                response.Success = false;
                response.Errors.Add("'AppKey' is not correct.");
                return Unauthorized(response);
            }
            if (response.Success)
            {

                #region Model Binding
                List<Products> products = new List<Products>();
                foreach (var item in Request.Payload)
                {
                    //products = Helper.DictionaryToObject(products.GetType(), item.Value) as List<Products>;

                }

                if (products != null && products.Count > 0)
                {
                    foreach (var item in Request.Payload)
                    {
                        if (item.Key == "Picture")
                        {

                        }
                        if (item.Key == "Fashion")
                        {

                        }
                    }
                }
                #endregion
                // Product Update Opertation
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Products")]
        public ActionResult<ResponseModel> ProductsUpdate([FromBody] RequestModel Request)
        {
            var response = new ResponseModel()
            {
                Success = true,
                Errors = new List<string>()
            };

            if (Request == null)
                throw new Exception("The request is invalid. cannot be empty.");

            if (Request.AppKey == null)
                throw new Exception("The request is invalid. The 'AppKey' area cannot be empty.");

            if (Request.Payload == null)
                throw new Exception("The request is invalid. The 'Payload' area cannot be empty.");


            bool isExist = ConnectionService.GetTenant(Request.AppKey);
            if (!isExist)
            {
                response.Success = false;
                response.Errors.Add("'AppKey' is not correct.");
                return Unauthorized(response);
            }
            if (response.Success)
            {
                // Product Insert Opertation
            }

            return Ok(response);
        }

        [HttpPatch]
        [Route("Products")]
        public ActionResult<ResponseModel> ProductsUpdateStatus([FromBody] RequestModel Request)
        {
            var response = new ResponseModel()
            {
                Success = true,
                Errors = new List<string>()
            };

            if (Request == null)
                throw new Exception("The request is invalid. cannot be empty.");

            if (Request.AppKey == null)
                throw new Exception("The request is invalid. The 'AppKey' area cannot be empty.");

            if (Request.Payload == null)
                throw new Exception("The request is invalid. The 'Payload' area cannot be empty.");


            bool isExist = ConnectionService.GetTenant(Request.AppKey);
            if (!isExist)
            {
                response.Success = false;
                response.Errors.Add("'AppKey' is not correct.");
                return Unauthorized(response);
            }
            if (response.Success)
            {
                // Product Update Status Opertation
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Products")]
        public ActionResult<ResponseModel> ProductsDelete([FromBody] RequestModel Request)
        {
            var response = new ResponseModel()
            {
                Success = true,
                Errors = new List<string>()
            };

            if (Request == null)
                throw new Exception("The request is invalid. cannot be empty.");

            if (Request.AppKey == null)
                throw new Exception("The request is invalid. The 'AppKey' area cannot be empty.");

            if (Request.Payload == null)
                throw new Exception("The request is invalid. The 'Payload' area cannot be empty.");


            bool isExist = ConnectionService.GetTenant(Request.AppKey);
            if (!isExist)
            {
                response.Success = false;
                response.Errors.Add("'AppKey' is not correct.");
                return Unauthorized(response);
            }
            if (response.Success)
            {
                // Product Delete Opertation
            }

            return Ok(response);
        }


    }
}
