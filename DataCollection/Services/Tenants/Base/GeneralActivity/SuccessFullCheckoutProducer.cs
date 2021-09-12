﻿using DataCollection.Contracts;
using DataCollection.Contracts.Entites;
using DataCollection.Entities.Base;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Services.Tenants.Base.GeneralActivity.RequestModels;
using DataCollection.Validator.ActivityValidator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base.GeneralActivity
{
    public class SuccessFullCheckoutProducer : ITenantService
    {
        private readonly IPackageService packageService;
        public SuccessFullCheckoutProducer(IPackageService packageService)
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
                SuccessFullCheckoutParams Params = new SuccessFullCheckoutParams();
                Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as SuccessFullCheckoutParams;
                foreach (var item in Action.Payload)
                {
                    if (item.Key == "OrderedItems")
                    {
                        Params.OrderedItems = JsonConvert.DeserializeObject<List<OrderedItem>>(item.Value.ToString());
                        break;
                    }
                }

                #endregion

                #region Validations

                response.Errors = Params.ValidateModel(new SuccessFullCheckoutValidator());

                if (string.IsNullOrWhiteSpace(Params.SessionID) && string.IsNullOrEmpty(Params.UserID))
                    response.Errors.Add("SessionID or UserID  at least one is required.");
                if (!string.IsNullOrEmpty(Params.DeliveryType) && (Params.DeliveryType != "Standart" && Params.DeliveryType != "In-store" && Params.DeliveryType != "Fast"))
                    response.Errors.Add("DeliveryType property must be 'Standart' , 'In-store' or 'Fast'");

                if (Params.OrderedItems.Count > 0)
                {
                    foreach (var item in Params.OrderedItems)
                    {
                        if (string.IsNullOrEmpty(item.Color)) response.Errors.Add($"Product with {item.ProductID} Id is missing color.");
                        if (string.IsNullOrEmpty(item.Size)) response.Errors.Add($"Product with {item.ProductID} Id is missing size.");
                        if (string.IsNullOrEmpty(item.ProductID)) response.Errors.Add($"ProductID is required for OrderedItems");
                        if (item.Quantity == 0) response.Errors.Add($"Product quantity with ProductID {item.ProductID} must be greater than 0");
                    }
                }

                response.Success = response.Errors.Count <= 0;
                if (!response.Success) return response;

                #endregion

                for (int i = 0; i < 500; i++)
                {
                    packageService.PurchaseList().Add(Params);
                }
                //packageService.PurchaseList().Add(Params);

                if (packageService.PurchaseList().Count > 499)
                {
                    #region Send Queue
                    PurchasePackage package = new PurchasePackage();
                    package.PackagePurchase = packageService.PurchaseList();

                    var bus = BusConfigurator.ConfigureBus();
                    string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
                    var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
                    sendEndPoint.Send<PurchasePackage>(package).Wait();


                    packageService.ClearPurchaseList();
                    #endregion
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
