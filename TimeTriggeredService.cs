using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Company.Function
{
    public class TimeTriggeredService
    {
        [FunctionName("TimeTriggeredService")] //Cron job is running each minute
        [return: ServiceBus("save_tracking_queue", Connection = "ServiceBusConnection")]
        public void Run(
            [TimerTrigger("0 * * * * *")]TimerInfo myTimer,
            [CosmosDB(
                databaseName: "escrow-db",
                collectionName: "tracking_numbers",
                ConnectionStringSetting = "CosmosConnectionString",
                SqlQuery = "SELECT * FROM c")]
                IEnumerable<TrackingDTO> trackingDTOs,
            [ServiceBus("request_package_status")] ICollector<TrackingDTO> queue,
             ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            foreach (TrackingDTO trackingDTO in trackingDTOs) {
                log.LogInformation($"Fetched {trackingDTO.trackAndTrace} from the database, and sending it to a queue");
                queue.Add(trackingDTO);
            }
        }
    }
}
