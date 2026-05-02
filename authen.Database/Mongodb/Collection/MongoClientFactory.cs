using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authen.Database.Mongodb.Collection
{
    public interface IMongoClientFactory
    {
        MongoDBContext GetClientDatabase(string databaseName);
        IMongoDatabase GetDatabase(string tenantId);
    }
    public class MongoClientFactory : IMongoClientFactory
    {

        private readonly IConfiguration _config;
        private readonly MongoClient _client;
        public MongoClientFactory(IConfiguration config)
        {
            _config = config;
            _client = new MongoClient(_config.GetConnectionString("MongoDB"));

        }

      

        public MongoDBContext GetClientDatabase(string databaseName)
        {
            var mongoDatabase = _client.GetDatabase(
             databaseName);
            if (mongoDatabase == null)
            {
                return null;
            }
            return new MongoDBContext(mongoDatabase);
        }

        public IMongoDatabase GetDatabase(string tenantId)
        {
            return GetClientDatabase(tenantId)?._database;
        }

    }
}
