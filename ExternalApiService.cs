using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class ExternalApiService
    {
        [FunctionName("ExternalApiService")]
        [return: ServiceBus("package_delivered_topic", Connection = "ServiceBusConnection")]
        public TrackingDTO Run(
            [ServiceBusTrigger("request_package_status_queue", Connection = "ServiceBusConnection")]TrackingDTO trackingDTO, 
            ILogger log)
        {
            log.LogInformation($"C# Requesting status of track and trace number: {trackingDTO.trackAndTrace}");
            //TODO: Make the actual API call
            log.LogInformation($"C# The package was delivered, so sending the message to a topic");
            return trackingDTO;
        }
    }
}
