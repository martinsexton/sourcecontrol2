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
    public class HandleTimesheetNoteCreated : INotificationHandler<TimesheetNoteCreated>
    {
        private IEmailService _emailService;

        public HandleTimesheetNoteCreated(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(TimesheetNoteCreated notification, CancellationToken cancellationToken)
        {
            _emailService.SendMail("doneill@hotmail.com", notification.UserEmail, "New Timesheet Note Created", notification.NoteDetails, "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
    }
}
