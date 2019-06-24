using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RegulationInput
{
    public static class SendToDB
    {
        [FunctionName("SendToDB")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            var postBody = req.Content.ReadAsAsync<Regulation>().Result;

        }
    }

    public class Regulation
    {
        public string RegTitle { get; set; }
        public string SubjectArea { get; set; }
        public string Description { get; set; }
        public string Jurisdiction { get; set; }
        public string RegType { get; set; }
    }
}
