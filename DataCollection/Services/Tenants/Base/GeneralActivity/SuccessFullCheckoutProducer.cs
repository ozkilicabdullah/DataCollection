using DataCollection.Contracts;
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

        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Errors = new List<string>()
            };

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

            if (string.IsNullOrWhiteSpace(Params.SessionId) && string.IsNullOrEmpty(Params.UserId))
                response.Errors.Add("SessionId or UserID  at least one is required.");

            response.Success = response.Errors.Count <= 0;
            if (!response.Success) return response;

            #endregion

            #region Send Queue

            var bus = BusConfigurator.ConfigureBus();
            string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
            var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
            sendEndPoint.Send<SuccessFullCheckoutParams>(Params).Wait();

            #endregion

            return response;

        }
    }
}
