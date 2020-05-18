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
    public class HandleTimesheetRejected : INotificationHandler<TimesheetRejected>
    {
        private IEmailService _emailService;

        public HandleTimesheetRejected(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(TimesheetRejected notification, CancellationToken cancellationToken)
        {
            string msg = string.Format("Your timesheet for week beginning {0} has been rejected", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", notification.UserEmail, "Timesheet Rejected", msg, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
    }
}
