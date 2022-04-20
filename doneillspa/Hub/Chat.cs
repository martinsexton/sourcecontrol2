using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace hub
{
    public class Chat : Hub
    {
        public void SendMessage(string method, string msg)
        {
           Clients.All.SendAsync(method, msg);
        }
    }
}