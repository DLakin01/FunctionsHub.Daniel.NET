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
        public const string DB_NAME = "globaldb";

        [FunctionName("SendToDB")]
        public static async Task SendToDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            var postBody = req.Content.ReadAsAsync<Regulation>().Result;

            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(MONGO_HOST, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            settings.ConnectionMode = ConnectionMode.Direct;

            MongoIdentity identity = new MongoInternalIdentity(DB_NAME, USERNAME);
            MongoIdentityEvidence evidence = new PasswordEvidence(PASSWORD);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            var mongo = new MongoClient(settings);
            var dbs = mongo.ListDatabasesAsync().Result.ToListAsync().Result;

            //var db = mongo.GetDatabase(DB_NAME);

            //var collections = db.ListCollectionsAsync().Result.ToListAsync().Result;
            //if (!collections.Any(c => c.ToString() == postBody.CollectionName))
            //{
            //    await db.CreateCollectionAsync(postBody.CollectionName);
            //}

            //IMongoCollection<Regulation> collection = db.GetCollection<Regulation>(postBody.CollectionName);
            //collection.InsertOne(postBody);
        }

        //[FunctionName("QueryDB")]
        //public static HttpResponseMessage QueryDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req, ILogger log)
        //{
        //    var collectionVertical = req.GetQueryNameValuePairs().FirstOrDefault(c => string.Compare(c.Key, "collection") == 0).Value;

        //    var mongo = new MongoClient(MONGO_URI);
        //    var db = mongo.GetDatabase(DB_NAME);
        //    IMongoCollection<Regulation> collection = db.GetCollection<Regulation>(collectionVertical);

        //    var collectionJson = JsonConvert.SerializeObject(collection);
        //    var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(collectionJson, Encoding.UTF8, "application/json") };
        //    return response;
        //}
    }

    public class Regulation
    {
        public string RegTitle { get; set; }
        public string CollectionName { get; set; }
        public string RegText { get; set; }
        public string Jurisdiction { get; set; }
        public string RegType { get; set; }
    }
}
