using DataCollection.Contracts;
using DataCollection.Contracts.MongoModels;
using DataCollection.Entities.Base;
using DataCollection.Model;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DataCollection.Services
{
    public interface IConnectionService
    {
        User GetCurrentUser(string UserName, string Password);
        User GetUserForClientId(string ClientId);
        IMongoCollection<IActivityModelBase> GetTenantCollection(string ConnectionKey, string CurrentCollectionName);
        IMongoCollection<IActivityModelBase> GetBaseCollection(string CurrentCollectionName);
        bool GetTenant(string appKey);
        bool SetupProject(string tenant);
        List<BasketParams> BasketList();
    }
}
