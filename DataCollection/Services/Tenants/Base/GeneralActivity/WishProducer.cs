using DataCollection.Contracts;
using DataCollection.Contracts.Entites;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Validator.ActivityValidator;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base
{
    public class WishProducer : ITenantService
    {

        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {

            var response = new ResponseModel
            {
                Errors = new List<string>()
            };

            #region Model Binding

            ActionRequest Action = Payload["Action"] as ActionRequest;
            WishParams Params = new WishParams();
            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as WishParams;
            foreach (var item in Action.Payload)
            {
                if (item.Key == "WishInfo")
                {
                    Params.WishInfo = JsonConvert.DeserializeObject<WishInfo>(item.Value.ToString());
                    break;
                }
            }

            #endregion

            #region Validations

            response.Errors = Params.ValidateModel(new WishValidator());

            if (string.IsNullOrWhiteSpace(Params.SessionID) && string.IsNullOrEmpty(Params.UserID))
                response.Errors.Add("SessionID or UserID  at least one is required.");
            if (string.IsNullOrEmpty(Params.Type) || (Params.Type != "Add" && Params.Type != "Remove" && !string.IsNullOrEmpty(Params.Type)))
                response.Errors.Add("Type must be 'Add' or 'Remove'");
            if (Params.Type == "Add")
                if (Params.WishInfo == null) response.Errors.Add("WishInfo is required.");
            if (Params.WishInfo.CurrentPrice == 0) response.Errors.Add("CurrentPrice is required.");


            response.Success = response.Errors.Count <= 0;
            if (!response.Success) return response;

            #endregion

            #region Send Queue

            var bus = BusConfigurator.ConfigureBus();
            string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
            var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
            sendEndPoint.Send<WishParams>(Params).Wait();

            #endregion

            return response;
        }
    }
}
