using DataCollection.Contracts;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Validator.NewFolder.ActivityValidator;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.GeneralActivity
{
    public class ViewProducer : ITenantService
    {
        public async Task<ResponseModel> Execute(Dictionary<string, object> Payload, string Identifer)
        {
            var response = new ResponseModel
            {
                Errors = new List<string>()
            };
            #region Model Binding

            ActionRequest Action = Payload["Action"] as ActionRequest;
            ViewParams Params = new ViewParams();
            Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as ViewParams;

            #endregion

            #region Validations

            response.Errors = Params.ValidateModel(new ViewValidator());

            if (string.IsNullOrWhiteSpace(Params.SessionId) && string.IsNullOrEmpty(Params.UserId))
                response.Errors.Add("SessionId or UserID  at least one is required");
            if (!string.IsNullOrEmpty(Params.Value) && (Params.Type != "Product" && Params.Type != "Catalog"))
                response.Errors.Add("Type property must be 'Product' or 'Catalog'");

            response.Success = response.Errors.Count <= 0;
            if (!response.Success) return response;

            #endregion

            #region Send Queue

            var bus = BusConfigurator.ConfigureBus();
            string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
            var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
            sendEndPoint.Send<ViewParams>(Params).Wait();

            #endregion

            return response;
        }
    }
}
