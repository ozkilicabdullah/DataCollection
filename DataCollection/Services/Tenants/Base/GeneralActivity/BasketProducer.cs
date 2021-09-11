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
        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Errors = new List<string>()
            };

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
                    if (string.IsNullOrEmpty(Params.BasketInfo.CurrentPrice)) response.Errors.Add("CurrentPrice is required");
                    else
                    {
                        float output = float.NaN;
                        float.TryParse(Params.BasketInfo.CurrentPrice, out output);
                        if (output == float.NaN) response.Errors.Add("CurrenPrice is not valid.");
                    }
                    if (string.IsNullOrEmpty(Params.BasketInfo.Color)) response.Errors.Add("Color is required.");
                    if (string.IsNullOrEmpty(Params.BasketInfo.Size)) response.Errors.Add("Size is required.");
                }
            }

            response.Success = response.Errors.Count <= 0;
            if (!response.Success) return response;

            #endregion

            #region Send Queue

            var bus = BusConfigurator.ConfigureBus();
            string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
            var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
            sendEndPoint.Send<BasketParams>(Params).Wait();

            #endregion

            return response;
        }
    }
}
