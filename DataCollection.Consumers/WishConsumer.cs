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
    public class WishConsumer : IConsumer<WishParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionWish";

        public WishConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }
        public async Task Consume(ConsumeContext<WishParams> context)
        {
            var Model = context.Message;

            try
            {
                PackageService.WishList().Add(Model);

                int ListLimit = Configuration.GetValue<int>("ListLimitWish");
                int ListCount = PackageService.WishList().Count;
                if (ListCount > ListLimit)
                {
                    List<WishParams> wishItems = PackageService.WishList();
                    List<WishRecordModel> recordModels = new List<WishRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);
                    foreach (var item in wishItems)
                    {
                        WishRecordModel recordItem = new WishRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            ProductID = item.ProductID,
                            Type = item.Type,
                            WishInfo = item.WishInfo,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey

                        };
                        recordModels.Add(recordItem);
                    }
                    await collection.InsertManyAsync(recordModels);

                    PackageService.ClearWishList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
