using DataCollection.Contracts.Entites.Base;
using DataCollection.Contracts.MongoModels;
using DataCollection.Helpers;
using DataCollection.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace DataCollection.Services
{

    public class ConnectionService : IConnectionService
    {
        private readonly IConfiguration _configuration;
        public ConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //private string ConnectionString = configuration.GetValue<string>();//string.Concat("mongodb://localhost:27017/DataCollection?retryWrites=true&w=majority");

        /// <summary>
        /// Get Main Database
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        private IMongoDatabase GetDatabase(string CollectionName)
        {
            string ConnectionString = _configuration.GetValue<string>("ConnnectionStringCollectionBase");

            var settings = MongoClientSettings.FromConnectionString(ConnectionString);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(CollectionName); //Database Name

            return database;
        }

        public User GetCurrentUser(string UserName, string Password)
        {
            IMongoDatabase collectionDb = GetDatabase("DataCollectionTest");
            IMongoCollection<User> collection = collectionDb.GetCollection<User>("Users"); //TableName
            User user = collection.Find(x => x.Username == UserName && x.Password == Password).FirstOrDefault();

            return user;
        }

        public IMongoCollection<IActivityModelBase> GetTenantCollection(string ConnectionKey, string CurrentCollectionName)
        {
            #region Get Main DataCollectionDB

            IMongoDatabase collectionDb = GetDatabase("DataCollection");
            IMongoCollection<CollectionBaseModel> collection = collectionDb.GetCollection<CollectionBaseModel>("CollectionBase"); //TableName
            CollectionBaseModel tenantDbInfo = collection.Find(x => x.ConnectionKey == ConnectionKey).FirstOrDefault();

            #endregion

            #region Connect to Collection
            if (tenantDbInfo == null)
                throw new Exception("Data Store is not found!");

            IMongoDatabase tenantDb = GetDatabase(string.Concat("DataCollection", tenantDbInfo.TenantName));
            IMongoCollection<IActivityModelBase> currentCollection = tenantDb.GetCollection<IActivityModelBase>(CurrentCollectionName);
            return currentCollection;

            #endregion
        }

        public bool GetTenant(string appKey)
        {
            IMongoDatabase collectionDb = GetDatabase("DataCollectionTest");
            IMongoCollection<CollectionBase> collection = collectionDb.GetCollection<CollectionBase>("CollectionBase"); //TableName
            CollectionBase isExist = collection.Find(x => x.ConnectionKey == SecureOperations.Decrypt(appKey)).FirstOrDefault();
            if (isExist != null)
                return true;
            else
                return false;
        }

        public bool SetupProject()
        {
            #region  ProjectSetup
            IMongoDatabase collectionDb = GetDatabase("DataCollection");

            // Kullanıcı Oluşturma
            IMongoCollection<User> collectionUser = collectionDb.GetCollection<User>("Users"); //TableName
            User newUser = new User
            {
                _Id = ObjectId.GenerateNewId(),
                Username = "swadmin",
                Password = "swadmin",
                Name = "Sorsware Developers",
                Role = "superadmin",
                Email = "info@sorsware.com"
            };
            collectionUser.InsertOne(newUser);

            // Kiracı - Firma oluşturmak
            IMongoCollection<CollectionBase> collectionBase = collectionDb.GetCollection<CollectionBase>("CollectionBase"); //TableName
            CollectionBase newRecord = new CollectionBase
            {
                _Id = ObjectId.GenerateNewId(),
                TenantName = "Sorsware",
                ConnectionKey = SecureOperations.Encrypt("SorswareKey")
            };
            collectionBase.InsertOne(newRecord);

            #endregion
            return true;
        }
    }
}
