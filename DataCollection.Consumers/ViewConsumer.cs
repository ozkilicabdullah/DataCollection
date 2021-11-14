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
    public class ViewConsumer : IConsumer<ViewParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionView";
        public ViewConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }
        public async Task Consume(ConsumeContext<ViewParams> context)
        {
            var Model = context.Message;
            try
            {
                PackageService.ViewList().Add(Model);

                int ListLimit = Configuration.GetValue<int>("ListLimitView");
                int ListCount = PackageService.ViewList().Count;
                if (ListCount > ListLimit)
                {
                    List<ViewParams> viewItems = PackageService.ViewList();
                    List<ViewRecordModel> recordModels = new List<ViewRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);
                    foreach (var item in viewItems)
                    {
                        ViewRecordModel recordItem = new ViewRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            Type = item.Type,
                            Value = item.Value,
                            ViewRange = item.ViewRange,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey
                        };
                        recordModels.Add(recordItem);
                    }
                    await collection.InsertManyAsync(recordModels);
                    PackageService.ClearViewList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
