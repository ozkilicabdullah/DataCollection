using DataCollection.Consumers.Service.PackageService;
using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class ReturnedConsumer : IConsumer<ReturnedParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionReturned";
        public ReturnedConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }
        public async Task Consume(ConsumeContext<ReturnedParams> context)
        {
            var Model = context.Message;

            try
            {

                PackageService.ReturnedList().Add(Model);
                int ListLimit = Configuration.GetValue<int>("ListLimitReturned");
                int ListCount = PackageService.ReturnedList().Count;
                if (ListCount > ListLimit)
                {
                    List<ReturnedParams> returnedItems = PackageService.ReturnedList();
                    List<ReturnedRecordModel> record = new List<ReturnedRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);
                    foreach (var item in returnedItems)
                    {
                        ReturnedRecordModel recorditem = new ReturnedRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            ReturnedProduct = item.ReturnedProduct,
                            PartialReturn = item.PartialReturn,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey
                        };
                        record.Add(recorditem);
                    }
                    await collection.InsertManyAsync(record);
                    PackageService.ClearReturnedList();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
