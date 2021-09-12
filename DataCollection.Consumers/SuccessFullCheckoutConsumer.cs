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
    public class SuccessFullCheckoutConsumer : IConsumer<PurchasePackage>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionSuccessFullCheckOut";
        public SuccessFullCheckoutConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<PurchasePackage> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.PackagePurchase[0].AppKey, CollectionName);
                List<SuccessFullCheckoutRecordModel> recordModel = new List<SuccessFullCheckoutRecordModel>();
                foreach (var item in Model.PackagePurchase)
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
                        CreatedOn = item.CreatedOn
                    };
                    recordModel.Add(recordItem);
                }

                await collection.InsertManyAsync(recordModel);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
