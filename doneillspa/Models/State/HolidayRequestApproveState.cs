using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

namespace doneillspa.Models.State
{
    public class HolidayRequestApproveState : IHolidayRequestState
    {
        private HolidayRequest context;

        public HolidayRequestApproveState(HolidayRequest request)
        {
            context = request;
        }
        public void Approve(ICalendarService _calendarService)
        {
            //Holiday Request already in Approve State.
        }

        public void Reject()
        {
            throw new NotImplementedException();
        }

        public void Created(IEmailService _emailServie)
        {
            //Nothing to do on saving a holiday request that is already approved.
        }
    }
}
