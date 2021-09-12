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
    public class WishConsumer : IConsumer<PackageWish>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionWish";

        public WishConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        public async Task Consume(ConsumeContext<PackageWish> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.WishPackage[0].AppKey, CollectionName);
                List<WishRecordModel> recordModels = new List<WishRecordModel>();
                foreach (var item in Model.WishPackage)
                {
                    WishRecordModel recordItem = new WishRecordModel
                    {
                        _Id = ObjectId.GenerateNewId(),
                        SessionID = item.SessionID,
                        UserID = item.UserID,
                        ProductID = item.ProductID,
                        Type = item.Type,
                        WishInfo = item.WishInfo,
                        CreatedOn = item.CreatedOn

                    };
                    recordModels.Add(recordItem);
                }

                await collection.InsertManyAsync(recordModels);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
