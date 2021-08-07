using DataCollection.Contracts;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base.GeneralActivity.RequestModels
{
    public class ReturnedProducer : ITenantService
    {

        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Errors = new List<string>()
            };

            #region Model Binding

            ActionRequest Action = Payload["Action"] as ActionRequest;
            ReturnedParams Params = new ReturnedParams();
            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as ReturnedParams;
            foreach (var item in Action.Payload)
            {
                if (item.Key == "ProductID")
                {
                    Params.ProductID = JsonConvert.DeserializeObject<List<string>>(item.Value.ToString());
                    break;
                }
            }
            #endregion

            #region Validations

            if (string.IsNullOrWhiteSpace(Params.SessionId) && string.IsNullOrEmpty(Params.UserId))
                response.Errors.Add("SessionId or UserID  at least one is required.");

            if (!Params.PartialRefund && Params.ProductID.Count == 0)
                response.Errors.Add("ProductID is required.");
            // response.Errors = Params.ValidateModel(new ReturnedValidator());
            response.Success = response.Errors.Count <= 0;
            if (!response.Success) return response;

            #endregion

            #region Send Queue

            var bus = BusConfigurator.ConfigureBus();
            string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
            var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
            sendEndPoint.Send<ReturnedParams>(Params).Wait();

            #endregion

            return response;
        }
    }
}
