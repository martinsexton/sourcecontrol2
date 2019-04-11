using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Calendar;

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
    }
}
