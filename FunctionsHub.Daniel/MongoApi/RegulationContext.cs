using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using MongoApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoApi
{
    public class RegulationContext
    {
        private readonly IMongoDatabase _database = null;

        public RegulationContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Databasse);
        }

        public IMongoCollection<Regulation> Regulations(string collection)
        {
            return _database.GetCollection<Regulation>(collection);
        }
    }
}
