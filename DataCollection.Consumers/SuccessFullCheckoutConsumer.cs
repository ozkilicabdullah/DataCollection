using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class SuccessFullCheckoutConsumer : IConsumer<SuccessFullCheckoutParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionSuccessFullCheckOut";
        public SuccessFullCheckoutConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;

        }
        public async Task Consume(ConsumeContext<SuccessFullCheckoutParams> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);
                SuccessFullCheckoutRecordModel recordModel = new SuccessFullCheckoutRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionId = Model.SessionId,
                    UserId = Model.UserId,
                    CampaignId = Model.CampaignId,
                    CouponId = Model.CouponId,
                    DeliveryAddressId = Model.DeliveryAddressId,
                    IsFreeShipping = Model.IsFreeShipping,
                    DeliveryType = Model.DeliveryType,
                    OrderId = Model.OrderId,
                    PaymentTypeId = Model.PaymentTypeId,
                    Platform = Model.Platform,
                    OrderedItems = Model.OrderedItems,
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
