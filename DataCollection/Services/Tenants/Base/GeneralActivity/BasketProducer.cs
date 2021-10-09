using DataCollection.Contracts;
using DataCollection.Contracts.Entites;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Validator.ActivityValidator;
using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base.GeneralActivity
{
    public class BasketProducer : ITenantService
    {
        private readonly IPackageService packageService;

        public BasketProducer(IPackageService packageService)
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
                BasketParams Params = new BasketParams();
                Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as BasketParams;
                foreach (var item in Action.Payload)
                {
                    if (item.Key == "BasketInfo")
                    {
                        Params.BasketInfo = JsonConvert.DeserializeObject<BasketInfo>(item.Value.ToString());
                        break;
                    }
                }
                #endregion

                #region Validations

                response.Errors = Params.ValidateModel(new BasketValidator());

                if (string.IsNullOrWhiteSpace(Params.SessionID) && string.IsNullOrEmpty(Params.UserID))
                    response.Errors.Add("SessionID or UserID  at least one is required.");
                if (!string.IsNullOrEmpty(Params.Type) && (Params.Type != "Add" && Params.Type != "Remove"))
                    response.Errors.Add("Type property must be 'Product' or 'Catalog'");

                //Basket Details validation if Type 'Add'
                if (Params.Type == "Add")
                {
                    if (Params.BasketInfo == null) response.Errors.Add("BasketInfo is required.");
                    else
                    {
                        if (Params.BasketInfo.Quantity == 0) response.Errors.Add("Quantity must be greater than 0");
                        if (Params.BasketInfo.CurrentPrice == 0) response.Errors.Add("CurrentPrice is required");

                        if (string.IsNullOrEmpty(Params.BasketInfo.Color)) response.Errors.Add("Color is required.");
                        if (string.IsNullOrEmpty(Params.BasketInfo.Size)) response.Errors.Add("Size is required.");
                    }
                }

                response.Success = response.Errors.Count <= 0;
                if (!response.Success) return response;

                #endregion


                packageService.BasketList().Add(Params);

                if (packageService.BasketList().Count > 499)
                {
                    #region Send Queue
                    BasketPackage package = new BasketPackage();
                    package.package = packageService.BasketList();

                    var bus = BusConfigurator.ConfigureBus();
                    string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
                    var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
                    sendEndPoint.Send<BasketPackage>(package).Wait();

                    packageService.ClearBasketList();
                    #endregion
                }

            }
            catch (Exception ex)
            {

                throw;
            }



            return response;
        }
    }
}
