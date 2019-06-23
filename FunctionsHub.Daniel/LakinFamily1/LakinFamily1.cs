using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LakinFamily1
{
    public static class LakinFamily1
    {
        [FunctionName("LakinFamily1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req, ILogger log)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("Hello World!");
            return response;
        }
    }
}
