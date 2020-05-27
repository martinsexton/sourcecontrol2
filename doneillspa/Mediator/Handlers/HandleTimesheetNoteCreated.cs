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
    public class HandleTimesheetNoteCreated : INotificationHandler<TimesheetNoteCreated>
    {
        private IEmailService _emailService;
        private UserManager<ApplicationUser> _userManager;

        public HandleTimesheetNoteCreated(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public Task Handle(TimesheetNoteCreated notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            _emailService.SendMail("doneill@hotmail.com", user.Email, "New Timesheet Note Created", notification.NoteDetails, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
    }
}
