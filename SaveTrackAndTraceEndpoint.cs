using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class SaveTrackAndTraceEndpoint
    {
        [FunctionName("SaveTrackAndTraceEndpoint")]
        [return: ServiceBus("save_tracking_queue", Connection = "ServiceBusConnection")]
        public static async Task<String> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string trackAndTrace = data?.trackAndTrace ?? "unknown";
            return trackAndTrace;
        }
    }
}
