using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class ViewConsumer : IConsumer<ViewParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionView";
        public ViewConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<ViewParams> context)
        {
            var Model = context.Message;
            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);
                ViewRecordModel recordModel = new ViewRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionId = Model.SessionId,
                    UserId = Model.UserId,
                    Type = Model.Type,
                    Value = Model.Value,
                    ViewRange = Model.ViewRange,
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
