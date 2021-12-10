using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class SaveTrackAndTraceService
    {
        [FunctionName("SaveTrackAndTraceService")]
        public static void Run(
            [ServiceBusTrigger("save_tracking_queue", Connection = "ServiceBusConnection")]TrackingDTO trackingDTO, 
            [CosmosDB(
                databaseName: "escrow-db",
                collectionName: "tracking_numbers",
                ConnectionStringSetting = "CosmosConnectionString")]out dynamic document,
            ILogger log)
        {
            log.LogInformation($"C# Saving the received tracking dto to the db: {trackingDTO.trackAndTrace}");
            trackingDTO.id = System.Guid.NewGuid().ToString();
            document = trackingDTO;
        }
    }
}
