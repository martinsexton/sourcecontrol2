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
        QueueClient queueClient;
        private string QUEUE_NAME = "messages";

        public MessageQueue(IConfiguration configuration)
        {
            Configuration = configuration;
            var azureStorageConnectionString = Configuration["ConnectionStrings:StorageConnectionString"];
            queueClient = new QueueClient(azureStorageConnectionString, QUEUE_NAME);
        }

        public void SendMessage(string msg)
        {
            queueClient.SendMessage(msg);
        }

    }
}
