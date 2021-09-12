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
    public class ReturnedConsumer : IConsumer<ReturnedPackage>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionReturned";
        public ReturnedConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<ReturnedPackage> context)
        {
            var Model = context.Message;

            try
            {
                List<ReturnedRecordModel> record = new List<ReturnedRecordModel>();
                var collection = ConnectionService.GetTenantCollection(Model.PackageReturned[0].AppKey, CollectionName);
                foreach (var item in Model.PackageReturned)
                {
                    ReturnedRecordModel recorditem = new ReturnedRecordModel
                    {
                        _Id = ObjectId.GenerateNewId(),
                        SessionID = item.SessionID,
                        UserID = item.UserID,
                        ReturnedProduct = item.ReturnedProduct,
                        PartialReturn = item.PartialReturn,
                        CreatedOn = item.CreatedOn
                    };
                    record.Add(recorditem);
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
