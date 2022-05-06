using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.SignalR.Client;

namespace ProjectReportJob.Services
{
    public sealed class SignalRService : ISignalRService
    {
        private static ISignalRService instance = null;
        private static readonly object locker = new object();
        private HubConnection _connection;

        private SignalRService() { }

        public SignalRService(string uri)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(uri)
                .Build();

            _connection.StartAsync().Wait();
        }

        public void SendMessage(string method, string msg)
        {
            _connection.InvokeAsync("SendMessage", method, msg).Wait();
        }

        public static ISignalRService Instance
        {
            get
            {
                lock (locker)
                {
                    //if (instance == null)
                    //{
                    //    instance = new SignalRService(ConfigurationManager.ConnectionStrings["SignalRUri"].ConnectionString);
                    //}
                    instance = new SignalRService(ConfigurationManager.ConnectionStrings["SignalRUri"].ConnectionString);
                    return instance;
                }
            }
        }
    }
}
