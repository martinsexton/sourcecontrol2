using doneillspa.Mediator.Notifications;
using doneillspa.Models;
using doneillspa.Services.Email;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private UserManager<ApplicationUser> _userManager;

        public HandleTimesheetRejected(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public Task Handle(TimesheetRejected notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been rejected", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Rejected", msg, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
    }
}
