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
    internal class BasketConsumer : IConsumer<BasketPackage>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionBasket";

        public BasketConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        public async Task Consume(ConsumeContext<BasketPackage> context)
        {
            var Model = context.Message;

            try
            {
                List<BasketRecordModel> record = new List<BasketRecordModel>();
                var collection = ConnectionService.GetTenantCollection(Model.package[0].AppKey, CollectionName);
                //throw new Exception("ex");
                foreach (var item in Model.package)
                {
                    BasketRecordModel recordModel = new BasketRecordModel
                    {
                        _Id = ObjectId.GenerateNewId(),
                        SessionID = item.SessionID,
                        UserID = item.UserID,
                        BasketInfo = item.BasketInfo,
                        ProductID = item.ProductID,
                        Type = item.Type,
                        CreatedOn = item.CreatedOn
                    };
                    record.Add(recordModel);
                }

                await collection.InsertManyAsync(record);
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}