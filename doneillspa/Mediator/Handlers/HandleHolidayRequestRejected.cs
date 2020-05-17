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
    public class HandleHolidayRequestRejected : INotificationHandler<HolidayRequestRejected>
    {
        private IEmailService _emailService;

        public HandleHolidayRequestRejected(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(HolidayRequestRejected notification, CancellationToken cancellationToken)
        {
            _emailService.SendMail("doneill@hotmail.com", notification.UserEmail, "Holiday Request Rejected", string.Format("Your holiday request from {0} for {1} days, has been rejected", notification.FromDate, notification.Days), "", string.Empty, string.Empty);
            return Task.CompletedTask;
        }
    }
}
