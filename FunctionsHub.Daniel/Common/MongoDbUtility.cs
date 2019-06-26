using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Linq;

namespace Common
{
    public class MongoDbUtility<T>
    {
        public const string MONGO_URI = "mongodb://regstechmongotest:g21JjpzZw6fE7fJiByzozrsRwi5gIgtlwWrX5mIrp7oKbzNuvtBo5iYGoRNPSuElWTVGclXzoQqV5hf70w46yw==@regstechmongotest.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";

        private IMongoDatabase _database;
        private IMongoCollection<T> _collection;

        public MongoDbUtility(string databaseName)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(MONGO_URI));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            _database = mongoClient.GetDatabase(databaseName);
        }

        public void SetCollection(string collectionName)
        {
            _collection = _database.GetCollection<T>(collectionName);
        }

        public void InsertOne(T obj)
        {
            _collection.InsertOne(obj);
        }

        public void InsertOneAsync(T obj)
        {
            _collection.InsertOneAsync(obj);
        }

        public void InsertMany(List<T> documents)
        {
            _collection.InsertMany(documents);
        }

        public void InsertManyAsync(List<T> documents)
        {
            _collection.InsertManyAsync(documents);
        }

        public T FindOne(string param, string value)
        {
            var filter = Builders<T>.Filter.Eq(param, value);
            var document = _collection.Find(filter).FirstOrDefault();
            return document;
        }

        public List<T> FindMany(List<FilterDefinition<T>> query)
        {
            //Concatenate filters
            var concatFilter = Builders<T>.Filter.And(query);
            var resultList = _collection.Find(concatFilter).ToList();
            return resultList;
        }

        public List<T> FindAll()
        {
            return _collection.Find(new BsonDocument()).ToListAsync().Result;
        }
    }
}
