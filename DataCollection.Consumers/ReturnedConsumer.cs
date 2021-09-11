using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;

using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class ReturnedConsumer : IConsumer<ReturnedParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionReturned";
        public ReturnedConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<ReturnedParams> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);
                ReturnedRecordModel recordModel = new ReturnedRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionID = Model.SessionID,
                    UserID = Model.UserID,
                    ReturnedProduct = Model.ReturnedProduct,
                    PartialReturn = Model.PartialReturn,
                    CreatedOn = Model.CreatedOn
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
