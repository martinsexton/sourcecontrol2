using doneillspa.Mediator.Notifications;
using doneillspa.Models;
using doneillspa.Services.Email;
using hub;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Handlers
{
    public class HandleTimesheetSubmitted : INotificationHandler<TimesheetSubmitted>
    {
        IHubContext<Chat> _hub;
        private IEmailService _emailService;
        private UserManager<ApplicationUser> _userManager;

        public HandleTimesheetSubmitted(IHubContext<Chat> hub, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _hub = hub;
            _emailService = emailService;
            _userManager = userManager;
        }

        public Task Handle(TimesheetSubmitted notification, CancellationToken cancellationToken)
        {
            _hub.Clients.All.SendAsync("timesheetsubmitted", "Timesheet Submitted For " + notification.Username);

            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been successfully submitted", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Submitted", msg, "", string.Empty, string.Empty);
            return Task.CompletedTask;
        }
    }
}
