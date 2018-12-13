using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class EmailNotification : Notification
    {
        public string DestinationEmail { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
