using DataCollection.Contracts;
using DataCollection.Contracts.Entites;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Services.Tenants.Base.GeneralActivity.RequestModels;
using DataCollection.Validator.ActivityValidator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
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

            if (string.IsNullOrWhiteSpace(Params.SessionId) && string.IsNullOrEmpty(Params.UserId))
                response.Errors.Add("SessionId or UserID  at least one is required.");

            if (Params.Type == "Add")
                if (Params.WishInfo == null) response.Errors.Add("WishInfo is required.");
                else
                if (string.IsNullOrEmpty(Params.WishInfo.CurrentPrice)) response.Errors.Add("CurrentPrice is required.");
                else
                {
                    float output = float.NaN;
                    float.TryParse(Params.WishInfo.CurrentPrice, out output);
                    if (output == float.NaN) response.Errors.Add("CurrenPrice is not valid.");
                }

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
