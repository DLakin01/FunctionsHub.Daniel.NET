using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Common;

namespace RegulationInput
{
    public static class MongoHub
    {
        public const string DB_NAME = "AllRegs";
        private static MongoDbUtility<Regulation> _mongoUtility = new MongoDbUtility<Regulation>(DB_NAME);

        [FunctionName("SendToDB")]
        public static async Task SendToDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req, ILogger log)
        {
            var postBody = req.Content.ReadAsAsync<Regulation>().Result;
            _mongoUtility.SetCollection(postBody.CollectionName);
            _mongoUtility.InsertOne(postBody);
        }

        [FunctionName("QueryDB")]
        public static HttpResponseMessage QueryDB([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req, ILogger log)
        {
            var collectionVertical = req.GetQueryNameValuePairs().FirstOrDefault(c => string.Compare(c.Key, "collection") == 0).Value;
            _mongoUtility.SetCollection(collectionVertical);
            var allDocuments = _mongoUtility.FindAll();

            var collectionJson = JsonConvert.SerializeObject(allDocuments);
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(collectionJson, Encoding.UTF8, "application/json") };
            return response;
        }
    }
}
