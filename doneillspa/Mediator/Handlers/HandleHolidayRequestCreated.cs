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
    public class HandleHolidayRequestCreated : INotificationHandler<HolidayRequestCreated>
    {
        private IEmailService _emailService;

        public HandleHolidayRequestCreated(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(HolidayRequestCreated notification, CancellationToken cancellationToken)
        {
            _emailService.SendMail("doneill@hotmail.com", notification.ApproverEmail, "Holiday Request", string.Format("{0} has requested holiday from {1} for {2} days.",notification.UserName, notification.FromDate, notification.Days), "", string.Empty, string.Empty);
            return Task.CompletedTask;
        }
    }
}
