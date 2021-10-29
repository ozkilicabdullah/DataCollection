using DataCollection.Contracts;
using DataCollection.Helpers;
using DataCollection.Model.Request;
using DataCollection.Model.Response;
using DataCollection.Services.Tenants.Base.GeneralActivity.RequestModels;
using DataCollection.Validator.ActivityValidator;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataCollection.Services.Tenants.Base.GeneralActivity
{
    public class SearchProducer : ITenantService
    {
        private readonly IPackageService packageService;
        public SearchProducer(IPackageService packageService)
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
                SearchParams Params = new SearchParams();
                Params = Helper.DictionaryToObject(Params.GetType(), Action.Payload) as SearchParams;

                #endregion

                #region Validations

                response.Errors = Params.ValidateModel(new SearchValidator());

                if (string.IsNullOrWhiteSpace(Params.SessionID) && string.IsNullOrEmpty(Params.UserID))
                    response.Errors.Add("SessionID or UserID  at least one is required");

                response.Success = response.Errors.Count <= 0;
                if (!response.Success) return response;

                #endregion


                packageService.SearchList().Add(Params);

                if (packageService.SearchList().Count > 299)
                {
                    #region Send Queue
                    SearchPackage package = new SearchPackage();
                    package.PackageSearch = packageService.SearchList();

                    var bus = BusConfigurator.ConfigureBus();
                    string hostQueue = string.Concat(RabbitMqConsts.RabbitMqUri, Action.Action.ToLower(), "_queue");
                    var sendEndPoint = bus.GetSendEndpoint(new Uri(hostQueue)).Result;
                    sendEndPoint.Send<SearchPackage>(package).Wait();

                    #endregion

                    packageService.ClearSearchList();// Listenin temizlenmesi
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
