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
    internal class BasketConsumer : IConsumer<BasketParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionBasket";


        public BasketConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }
        public async Task Consume(ConsumeContext<BasketParams> context)
        {
            var Model = context.Message;

            try
            {
                PackageService.BasketList().Add(Model);

                int ListLimit = Configuration.GetValue<int>("ListLimitBasket");
                int ListCount = PackageService.BasketList().Count;

                if (ListCount > ListLimit)
                {
                    List<BasketParams> basketItems = PackageService.BasketList();
                    List<BasketRecordModel> record = new List<BasketRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);

                    foreach (var item in basketItems)
                    {
                        BasketRecordModel recordModel = new BasketRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            BasketInfo = item.BasketInfo,
                            ProductID = item.ProductID,
                            Type = item.Type,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey
                        };
                        record.Add(recordModel);
                    }
                    await collection.InsertManyAsync(record);
                    PackageService.ClearBasketList();
                }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}