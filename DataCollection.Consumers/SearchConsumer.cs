using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class SearchConsumer : IConsumer<SearchParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionSearch";
        public SearchConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<SearchParams> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);

                SearchRecordModel recordModel = new SearchRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionId = Model.SessionId,
                    UserId = Model.UserId,
                    Value = Model.Value,
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
