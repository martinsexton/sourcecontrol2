using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Notifications
{
    public class EmailNotificationCreated : INotification
    {
        public string DestinationEmail { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
