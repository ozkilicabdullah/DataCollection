using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class SearchConsumer : IConsumer<SearchPackage>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionSearch";
        public SearchConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<SearchPackage> context)
        {
            var Model = context.Message;

            try
            {
                //var collection = ConnectionService.GetTenantCollection(Model.PackageSearch[0].AppKey, CollectionName);
                var collection = ConnectionService.GetBaseCollection(CollectionName);
                List<SearchRecordModel> recordModel = new List<SearchRecordModel>();
                foreach (var item in Model.PackageSearch)
                {
                    SearchRecordModel recordItem = new SearchRecordModel
                    {
                        _Id = ObjectId.GenerateNewId(),
                        SessionID = item.SessionID,
                        UserID = item.UserID,
                        Value = item.Value,
                        CreatedOn = item.CreatedOn,
                        AppKey = item.AppKey
                    };
                    recordModel.Add(recordItem);
                }

                await collection.InsertManyAsync(recordModel);

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
