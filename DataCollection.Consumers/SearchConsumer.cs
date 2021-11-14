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
    public class SearchConsumer : IConsumer<SearchParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionSearch";
        public SearchConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }
        public async Task Consume(ConsumeContext<SearchParams> context)
        {
            var Model = context.Message;

            try
            {
                PackageService.SearchList().Add(Model);

                int ListLimit = Configuration.GetValue<int>("ListLimitSearch");
                int ListCount = PackageService.SearchList().Count;
                if (ListCount > ListLimit)
                {
                    List<SearchParams> sarchItems = PackageService.SearchList();
                    List<SearchRecordModel> recordModel = new List<SearchRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);
                    foreach (var item in sarchItems)
                    {
                        SearchRecordModel recordItem = new SearchRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            Value = item.Value,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey
                        };
                        recordModel.Add(recordItem);
                    }
                    await collection.InsertManyAsync(recordModel);
                    PackageService.ClearSearchList();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
