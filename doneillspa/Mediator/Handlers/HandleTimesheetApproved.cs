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
    public class HandleTimesheetApproved : INotificationHandler<TimesheetApproved>
    {
        private IEmailService _emailService;

        public HandleTimesheetApproved(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(TimesheetApproved notification, CancellationToken cancellationToken)
        {
            string msg = string.Format("Your timesheet for week beginning {0} has been approved", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", notification.UserEmail, "Timesheet Approved", msg, "", string.Empty, string.Empty);
            
            return Task.CompletedTask;
        }
    }
}
