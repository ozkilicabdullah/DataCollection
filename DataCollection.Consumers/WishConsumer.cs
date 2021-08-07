﻿using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Services;
using MassTransit;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace DataCollection.Consumers
{
    public class WishConsumer : IConsumer<WishParams>
    {
        private readonly IConnectionService ConnectionService;
        private string CollectionName = "CollectionWish";

        public WishConsumer(IConnectionService connectionService)
        {
            ConnectionService = connectionService;
        }
        public async Task Consume(ConsumeContext<WishParams> context)
        {
            var Model = context.Message;

            try
            {
                var collection = ConnectionService.GetTenantCollection(Model.AppKey, CollectionName);
                WishRecordModel recordModel = new WishRecordModel
                {
                    _Id = ObjectId.GenerateNewId(),
                    SessionId = Model.SessionId,
                    UserId = Model.UserId,
                    ProductId = Model.ProductId,
                    Type = Model.Type,
                    WishInfo = Model.WishInfo,
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
