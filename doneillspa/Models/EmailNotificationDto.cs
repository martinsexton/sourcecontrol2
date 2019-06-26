using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class EmailNotificationDto
    {
        public long Id { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string DestinationEmail { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
