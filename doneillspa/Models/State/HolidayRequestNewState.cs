using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

namespace doneillspa.Models.State
{
    public class HolidayRequestNewState : IHolidayRequestState
    {
        private HolidayRequest context;

        public HolidayRequestNewState(HolidayRequest request)
        {
            context = request;
        }
        public void Approve(ICalendarService _calendarService, IEmailService _emailService, ITimesheetService _timesheetService)
        {
            context.Status = HolidayRequestStatus.Approved;
            _calendarService.CreateEvent(context.FromDate, context.Days, context.User.FirstName + " " + context.User.Surname);
            _emailService.SendMail("doneill@hotmail.com", context.User.Email, "Holiday Request Approved", string.Format("Your holiday request from {0} for {1} days, has been approved", context.FromDate, context.Days), "", string.Empty, string.Empty);
            _timesheetService.RecordAnnualLeave(context.User.UserName, context.FromDate, context.Days);
        }

        public void Reject(IEmailService _emailService)
        {
            context.Status = HolidayRequestStatus.Rejected;
            _emailService.SendMail("doneill@hotmail.com", context.User.Email, "Holiday Request Rejected", string.Format("Your holiday request from {0} for {1} days, has been rejected", context.FromDate, context.Days), "", string.Empty, string.Empty);
        }

        public void Created(IEmailService _emailServie)
        {
            _emailServie.SendMail("doneill@hotmail.com", context.Approver.Email, "Holiday Request", string.Format("{0} has requested holiday from {1} for {2} days.", context.User.FirstName, context.FromDate, context.Days), "", string.Empty, string.Empty);
        }
    }
}
