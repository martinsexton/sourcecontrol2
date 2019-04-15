using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void Approve(ICalendarService _calendarService)
        {
            context.Status = HolidayRequestStatus.Approved;
            //Create an event in Google Calendar.
            _calendarService.CreateEvent(context.FromDate, context.Days, context.User.FirstName + " " + context.User.Surname);
        }

        public void Reject()
        {
            throw new NotImplementedException();
        }

        public void Created(IEmailService _emailServie)
        {
            _emailServie.SendMail("doneill@hotmail.com", context.Approver.Email, "Holiday Request", string.Format("{0} has requested holiday from {1} for {2} days.", context.User.FirstName, context.FromDate, context.Days), "", string.Empty, string.Empty);
        }
    }
}
