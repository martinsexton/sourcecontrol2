using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Mediator.Notifications;
using doneillspa.Services.Email;
using MediatR;

namespace doneillspa.Models
{
    public class EmailNotification : Notification
    {
        public string DestinationEmail { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public void Created(IMediator _mediator)
        {
            _mediator.Publish(new EmailNotificationCreated { DestinationEmail = this.DestinationEmail, Body = this.Body, Subject = this.Subject });
        }
    }
}
