using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    internal class BasketConsumer : IConsumer<BasketParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionBasket";

        public BasketConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        public async Task Consume(ConsumeContext<BasketParams> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);
                //throw new Exception("ex");
                BasketRecordModel recordModel = new BasketRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionID = Model.SessionID,
                    UserID = Model.UserID,
                    BasketInfo = Model.BasketInfo,
                    ProductID = Model.ProductID,
                    Type = Model.Type,
                    CreatedOn = DateTime.Now
                };
                await collection.InsertOneAsync(recordModel);
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}