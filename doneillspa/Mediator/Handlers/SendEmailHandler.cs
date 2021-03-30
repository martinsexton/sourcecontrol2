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
    public class SendEmailHandler : INotificationHandler<EmailNotificationCreated>, INotificationHandler<TimesheetApproved>, 
        INotificationHandler<TimesheetNoteCreated>, INotificationHandler<TimesheetRejected>, INotificationHandler<TimesheetSubmitted>
    {
        private IEmailService _emailService;
        private UserManager<ApplicationUser> _userManager;

        public SendEmailHandler(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        #region Handle Email Notification Event
        public Task Handle(EmailNotificationCreated notification, CancellationToken cancellationToken)
        {
            _emailService.SendMail("doneill@hotmail.com", notification.DestinationEmail, notification.Subject, notification.Body, notification.Body, string.Empty, string.Empty);
            return Task.CompletedTask;
        }
        #endregion

        #region Handle Timmesheet Approved
        public Task Handle(TimesheetApproved notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been approved", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Approved", msg, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
        #endregion

        #region Handle Timmmesheet Note Create
        public Task Handle(TimesheetNoteCreated notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            _emailService.SendMail("doneill@hotmail.com", user.Email, "New Timesheet Note Created", notification.NoteDetails, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
        #endregion

        #region Handle Timesheet Rejected
        public Task Handle(TimesheetRejected notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been rejected", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Rejected", msg, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
        #endregion

        #region Handle Timesheet Submmitted
        public Task Handle(TimesheetSubmitted notification, CancellationToken cancellationToken)
        {
            ApplicationUser user = _userManager.FindByIdAsync(notification.OwnerId).Result;
            string msg = string.Format("Your timesheet for week beginning {0} has been successfully submitted", notification.WeekStarting.ToShortDateString());
            _emailService.SendMail("doneill@hotmail.com", user.Email, "Timesheet Submitted", msg, "", string.Empty, string.Empty);
            return Task.CompletedTask;
        }
        #endregion

    }
}
