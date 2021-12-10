using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace Company.Function
{
    public class DeleteTrackAndTraceService
    {
        [FunctionName("DeleteTrackAndTraceService")]
        public async Task Run(
            [ServiceBusTrigger("package_delivered_topic", Connection = "ServiceBusConnection")]TrackingDTO trackingDTO,
            [CosmosDB(
                databaseName: "escrow-db",
                collectionName: "tracking_numbers",
                ConnectionStringSetting = "CosmosConnectionString")]
                DocumentClient documentClient,
            ILogger log)
        {
            log.LogInformation($"C# Deleting the tracking document with id: {trackingDTO.id} and track and trace number: {trackingDTO.trackAndTrace}");
            
            try {
                await documentClient.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri("escrow-db","tracking_numbers", trackingDTO.id),
                    new RequestOptions { PartitionKey = new PartitionKey(trackingDTO.id) }
                    );
            } catch(Exception e) {
                //ignore failure for now
            }
        }  
    }
}
