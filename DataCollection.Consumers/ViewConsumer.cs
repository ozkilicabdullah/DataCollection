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
    public class ViewConsumer : IConsumer<ViewPackage>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionView";
        public ViewConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<ViewPackage> context)
        {
            var Model = context.Message;
            try
            {
                //var collection = ConnectionService.GetTenantCollection(Model.PackageView[0].AppKey, CollectionName);
                var collection = ConnectionService.GetBaseCollection(CollectionName);
                List<ViewRecordModel> recordModels = new List<ViewRecordModel>();
                foreach (var item in Model.PackageView)
                {
                    ViewRecordModel recordItem = new ViewRecordModel
                    {
                        _Id = ObjectId.GenerateNewId(),
                        SessionID = item.SessionID,
                        UserID = item.UserID,
                        Type = item.Type,
                        Value = item.Value,
                        ViewRange = item.ViewRange,
                        CreatedOn = item.CreatedOn,
                        AppKey = item.AppKey
                    };
                    recordModels.Add(recordItem);
                }

                await collection.InsertManyAsync(recordModels);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
