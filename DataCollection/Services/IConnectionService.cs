﻿using DataCollection.Contracts.MongoModels;
using DataCollection.Entities.Base;
using DataCollection.Model;
using MongoDB.Driver;


namespace DataCollection.Services
{
    public interface IConnectionService
    {
        User GetCurrentUser(string UserName, string Password);
        IMongoCollection<IActivityModelBase> GetTenantCollection(string ConnectionKey, string CurrentCollectionName);
        bool GetTenant(string appKey);
        bool SetupProject();
    }
}
