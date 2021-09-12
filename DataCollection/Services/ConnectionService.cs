﻿using DataCollection.Contracts;
using DataCollection.Contracts.Entites.Base;
using DataCollection.Contracts.MongoModels;
using DataCollection.Helpers;
using DataCollection.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DataCollection.Services
{

    public class ConnectionService : IConnectionService
    {
        private readonly IConfiguration _configuration;
        private string privateKey = "DcSOrsBt0794";
        public List<BasketParams> ListBasket = new List<BasketParams>();
        public ConnectionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
            var database = client.GetDatabase(CollectionName); //Database Names

            return database;
        }
        /// <summary>
        /// Get Current User
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public User GetCurrentUser(string UserName, string Password)
        {
            IMongoDatabase collectionDb = GetDatabase("DataCollection");
            IMongoCollection<User> collection = collectionDb.GetCollection<User>("Users"); //TableName
            User user = collection.Find(x => x.Username == UserName && x.Password == Password).FirstOrDefault();

            return user;
        }
        /// <summary>
        /// Get Tenant Collection
        /// </summary>
        /// <param name="ConnectionKey"> App Key</param>
        /// <param name="CurrentCollectionName"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Are there any Tenant for AppKey
        /// </summary>
        /// <param name="appKey">Tenant private key</param>
        /// <returns></returns>
        public bool GetTenant(string appKey)
        {
            IMongoDatabase collectionDb = GetDatabase("DataCollection");
            IMongoCollection<CollectionBase> collection = collectionDb.GetCollection<CollectionBase>("CollectionBase"); //TableName
            CollectionBase isExist = collection.Find(x => x.ConnectionKey == appKey).FirstOrDefault();
            if (isExist != null)
                return true;
            else
                return false;
        }
        // Project Setup
        public bool SetupProject(string tenant)
        {
            if (string.IsNullOrEmpty(tenant))
                tenant = "Sorsware";
            #region  ProjectSetup
            IMongoDatabase collectionDb = GetDatabase("DataCollection");

            // Kullanıcı Oluşturma
            IMongoCollection<User> collectionUser = collectionDb.GetCollection<User>("Users"); //TableName
            User newUser = new User
            {
                _Id = ObjectId.GenerateNewId(),
                Username = string.Concat(tenant, "SorsBtAdmin"),
                Password = string.Concat(tenant, "SorsBtAdmin"),
                Name = "Sorsware Developers",
                Role = "superadmin",
                Email = "info@sorsware.com",
                ClientId = SecureOperations.Encrypt(string.Concat(tenant, privateKey))
            };
            collectionUser.InsertOne(newUser);

            // Kiracı - Firma oluşturmak
            IMongoCollection<CollectionBase> collectionBase = collectionDb.GetCollection<CollectionBase>("CollectionBase"); //TableName
            CollectionBase newRecord = new CollectionBase
            {
                _Id = ObjectId.GenerateNewId(),
                TenantName = tenant,
                ConnectionKey = SecureOperations.Encrypt(string.Concat(tenant, privateKey))
            };
            collectionBase.InsertOne(newRecord);

            #endregion
            return true;
        }
        /// <summary>
        /// Client Id = Frim AppKey
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public User GetUserForClientId(string ClientId)
        {
            IMongoDatabase collectionDb = GetDatabase("DataCollection");
            IMongoCollection<User> collection = collectionDb.GetCollection<User>("Users"); //TableName
            User user = collection.Find(x => x.ClientId == ClientId).FirstOrDefault();
            return user;
        }

        public List<BasketParams> BasketList()
        {
            return ListBasket;
        }
    }
}
