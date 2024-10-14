using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services.MessageQueue
{
    public class MessageQueue : IMessageQueue
    {
        private IConfiguration Configuration;
        
        private string QUEUE_NAME = "livefixreports";
        private string azureStorageConnectionString = "";

        public MessageQueue(IConfiguration configuration)
        {
            Configuration = configuration;
            azureStorageConnectionString = Configuration["ConnectionStrings:StorageConnectionString"];
            
        }

        public void SendMessage(string msg)
        {
            QueueClient queueClient = new QueueClient(azureStorageConnectionString, QUEUE_NAME);
            queueClient.SendMessage(msg);
        }

    }
}
