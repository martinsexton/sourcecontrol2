using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services.MessageQueue
{
    public interface IMessageQueue
    {
        void SendMessage(string msg);
    }
}
