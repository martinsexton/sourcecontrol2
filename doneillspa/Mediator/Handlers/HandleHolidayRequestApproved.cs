using doneillspa.Mediator.Notifications;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Handlers
{
    public class HandleHolidayRequestApproved : INotificationHandler<HolidayRequestApproved>
    {
        private IEmailService _emailService;
        private ICalendarService _calendarService;
        private ITimesheetService _timesheetService;

        public HandleHolidayRequestApproved(IEmailService emailService, ICalendarService calendarService, ITimesheetService timesheetService)
        {
            _emailService = emailService;
            _calendarService = calendarService;
            _timesheetService = timesheetService;
        }

        public Task Handle(HolidayRequestApproved notification, CancellationToken cancellationToken)
        {
            _timesheetService.RecordAnnualLeave(notification.UserName, notification.FromDate, notification.Days);
            _calendarService.CreateEvent(notification.FromDate, notification.Days, notification.UserName);
            _emailService.SendMail("doneill@hotmail.com", notification.UserEmail, "Holiday Request Approved", string.Format("Your holiday request from {0} for {1} days, has been approved", notification.FromDate, notification.Days), "", string.Empty, string.Empty);

            return Task.CompletedTask;
        }
    }
}
