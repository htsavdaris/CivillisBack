using System.Xml.Serialization;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace CivilisApp.Utils
{
    public class QueueManager
    {
        public QueueManager() { }
        

        public async Task writeToQueueAsync(string message)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=civilisstorage;AccountKey=d2HpDhMwtlWkc2F6GGaPAgMXz/N9lADvJeKmf2ypuzinFviRPewA5E/IwY/B2ezFGXw0CQ4tpFfW+AStQ98NTA==;EndpointSuffix=core.windows.net";

            QueueClient queueClient = new QueueClient(connectionString, "reportgen");
            SendReceipt receipt = await queueClient.SendMessageAsync(message);
        }
    }
}
