using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace RegulationInput
{
    public static class MongoHub
    {
        public const string MONGO_URI = "mongodb://regstechmongotest:g21JjpzZw6fE7fJiByzozrsRwi5gIgtlwWrX5mIrp7oKbzNuvtBo5iYGoRNPSuElWTVGclXzoQqV5hf70w46yw==@regstechmongotest.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
        public const string MONGO_HOST = "regstechmongotest.documents.azure.com";
        public const string USERNAME = "regstechmongotest";
        public const string PASSWORD = "g21JjpzZw6fE7fJiByzozrsRwi5gIgtlwWrX5mIrp7oKbzNuvtBo5iYGoRNPSuElWTVGclXzoQqV5hf70w46yw==";
        public const string DB_NAME = "AllRegs";

        [FunctionName("SendToDB")]
        public static async Task SendToDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req, ILogger log)
        {
            var postBody = req.Content.ReadAsAsync<Regulation>().Result;

            string connectionString = "mongodb://regstechmongotest:3bXjjkRPtvlJtsxZqt2FRIUHaf9FBwcZicIZknCwcJcF8tNU2CrTXGaGq4pxNCNaT2XXlEzQFb7ARRCh6tMMTQ==@regstechmongotest.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongo = new MongoClient(settings);

            mongo.Cluster.StartSession();
            var dbs = mongo.ListDatabasesAsync().Result.ToListAsync().Result;

            var db = mongo.GetDatabase(DB_NAME);
            IMongoCollection<Regulation> collection = db.GetCollection<Regulation>(postBody.CollectionName);
            collection.InsertOne(postBody);
        }

        [FunctionName("QueryDB")]
        public static HttpResponseMessage QueryDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req, ILogger log)
        {
            var collectionVertical = req.GetQueryNameValuePairs().FirstOrDefault(c => string.Compare(c.Key, "collection") == 0).Value;

            string connectionString = "mongodb://regstechmongotest:3bXjjkRPtvlJtsxZqt2FRIUHaf9FBwcZicIZknCwcJcF8tNU2CrTXGaGq4pxNCNaT2XXlEzQFb7ARRCh6tMMTQ==@regstechmongotest.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongo = new MongoClient(settings);

            var db = mongo.GetDatabase(DB_NAME);
            IMongoCollection<Regulation> collection = db.GetCollection<Regulation>(collectionVertical);
            var allDocuments = collection.Find(new BsonDocument()).ToListAsync().Result;

            var collectionJson = JsonConvert.SerializeObject(allDocuments);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(collectionJson, Encoding.UTF8, "application/json") };
            return response;
        }
    }

    public class Regulation
    {
        public ObjectId _id { get; set; }
        public string RegTitle { get; set; }
        public string CollectionName { get; set; }
        public string RegText { get; set; }
        public string Jurisdiction { get; set; }
    }
}
