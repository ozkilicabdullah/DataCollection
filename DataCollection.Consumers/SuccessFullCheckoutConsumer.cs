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
    public class SuccessFullCheckoutConsumer : IConsumer<SuccessFullCheckoutParams>
    {
        private readonly IConnectionService ConnectionService;
        private readonly IPackageService PackageService;
        private readonly IConfiguration Configuration;

        private string CollectionName = "CollectionSuccessFullCheckOut";
        public SuccessFullCheckoutConsumer(IConnectionService connectionService, IPackageService packageService, IConfiguration configuration)
        {
            ConnectionService = connectionService;
            PackageService = packageService;
            Configuration = configuration;
        }

        public async Task Consume(ConsumeContext<SuccessFullCheckoutParams> context)
        {
            var Model = context.Message;

            try
            {
                PackageService.PurchaseList().Add(Model);
                int ListLimit = Configuration.GetValue<int>("ListLimitPurchase");
                int ListCount = PackageService.PurchaseList().Count;
                if (ListCount > ListLimit)
                {
                    List<SuccessFullCheckoutParams> purchaseItesm = PackageService.PurchaseList();
                    List<SuccessFullCheckoutRecordModel> recordModel = new List<SuccessFullCheckoutRecordModel>();
                    var collection = ConnectionService.GetBaseCollection(CollectionName);
                    foreach (var item in purchaseItesm)
                    {
                        SuccessFullCheckoutRecordModel recordItem = new SuccessFullCheckoutRecordModel
                        {
                            _Id = ObjectId.GenerateNewId(),
                            SessionID = item.SessionID,
                            UserID = item.UserID,
                            CampaignID = item.CampaignID,
                            CouponID = item.CouponID,
                            DeliveryAddressID = item.DeliveryAddressID,
                            IsFreeShipping = item.IsFreeShipping,
                            DeliveryType = item.DeliveryType,
                            OrderID = item.OrderID,
                            PaymentTypeID = item.PaymentTypeID,
                            Platform = item.Platform,
                            OrderedItems = item.OrderedItems,
                            CreatedOn = item.CreatedOn,
                            AppKey = item.AppKey
                        };
                        recordModel.Add(recordItem);
                    }
                    await collection.InsertManyAsync(recordModel);
                    PackageService.ClearPurchaseList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
