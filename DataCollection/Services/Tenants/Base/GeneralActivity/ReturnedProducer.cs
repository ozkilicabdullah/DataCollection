using DataCollection.Contracts;
using DataCollection.Contracts.Entites;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Validator.ActivityValidator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base.GeneralActivity.RequestModels
{
    public class ReturnedProducer : ITenantService
    {
        private readonly IPackageService packageService;
        public ReturnedProducer(IPackageService packageService)
        {
            this.packageService = packageService;
        }
        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Errors = new List<string>()
            };
            try
            {
                #region Model Binding

                ActionRequest Action = Payload["Action"] as ActionRequest;
                ReturnedParams Params = new ReturnedParams();
                Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as ReturnedParams;
                foreach (var item in Action.Payload)
                {
                    if (item.Key == "ReturnedProduct")
                    {
                        Params.ReturnedProduct = JsonConvert.DeserializeObject<List<ReturnedProduct>>(item.Value.ToString());
                        break;
                    }
                }
                #endregion

                #region Validations
                response.Errors = Params.ValidateModel(new ReturnedValidator());


                if (string.IsNullOrWhiteSpace(Params.SessionID) && string.IsNullOrEmpty(Params.UserID))
                    response.Errors.Add("SessionID or UserID  at least one is required.");

                if (!Params.PartialReturn && Params.ReturnedProduct == null) response.Errors.Add("ReturnedProduct are required for partial returnds");


                if (!Params.PartialReturn && Params.ReturnedProduct != null)
                {
                    if (Params.ReturnedProduct.Count == 0)
                        response.Errors.Add("Items are required for partial returnds");
                }

                if (!Params.PartialReturn && Params.ReturnedProduct != null)
                {
                    if (Params.ReturnedProduct.Count > 0)
                    {
                        foreach (var item in Params.ReturnedProduct)
                        {
                            if (string.IsNullOrEmpty(item.ProductID)) response.Errors.Add("ProductID is required for ReturnedProducts");
                            if (string.IsNullOrEmpty(item.Color)) response.Errors.Add($"Product with {item.ProductID} Id is missing color.");
                            if (string.IsNullOrEmpty(item.Size)) response.Errors.Add($"Product with {item.ProductID} Id is missing size.");
                        }
                    }
                }


                response.Success = response.Errors.Count <= 0;
                if (!response.Success) return response;

                #endregion


                packageService.ReturnedList().Add(Params);

                if (packageService.ReturnedList().Count > 49)
                {
                    #region Send Queue
                    ReturnedPackage package = new ReturnedPackage();
                    package.PackageReturned = packageService.ReturnedList();
                    var bus = BusConfigurator.ConfigureBus();
                    string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
                    var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
                    sendEndPoint.Send<ReturnedPackage>(package).Wait();

                    #endregion
                    // Listeyi temizle
                    packageService.ClearReturnedList();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return response;
        }
    }
}
