using doneillspa.Mediator.Notifications;
using doneillspa.Services.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Handlers
{
    public class HandleEmailNotificationCreated : INotificationHandler<EmailNotificationCreated>
    {
        private IEmailService _emailService;

        public HandleEmailNotificationCreated(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(EmailNotificationCreated notification, CancellationToken cancellationToken)
        {
            _emailService.SendMail("doneill@hotmail.com", notification.DestinationEmail, notification.Subject, notification.Body, notification.Body, string.Empty, string.Empty);
            return Task.CompletedTask;
        }
    }
}
