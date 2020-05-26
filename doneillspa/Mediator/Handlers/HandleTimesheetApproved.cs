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
    public class HandleTimesheetApproved : INotificationHandler<TimesheetApproved>
    {
        private IEmailService _emailService;
        private UserManager<ApplicationUser> _userManager;

        public HandleTimesheetApproved(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public Task Handle(TimesheetApproved notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been approved", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Approved", msg, "", string.Empty, string.Empty);
            
            return Task.CompletedTask;
        }
    }
}
