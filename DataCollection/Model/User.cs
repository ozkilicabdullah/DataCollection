using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DataCollection.Model
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public DateTime LastTokenDate { get; set; }
        public string LastToken { get; set; }
        public List<string> Perms { get; set; }
        /// <summary>
        /// App Key
        /// </summary>
        public string ClientId { get; set; }

    }
}