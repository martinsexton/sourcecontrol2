using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectReportJob.Services
{
    public interface ISignalRService
    {
        void SendMessage(string method, string msg);
    }
}
