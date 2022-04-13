using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace ProjectReportJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("messages")] GenerateReportEvent message, 
            [Blob("doneillreports/report.txt", FileAccess.Write)] TextWriter reportWriter, TextWriter log)
        {
            reportWriter.WriteLine($"Report created for { message.ProjectCode}");
        }
    }
}
