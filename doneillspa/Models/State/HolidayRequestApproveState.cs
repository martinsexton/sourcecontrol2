using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

namespace doneillspa.Models.State
{
    public class HolidayRequestApproveState : IHolidayRequestState
    {
        private HolidayRequest context;
        private ICalendarService _calendarService;
        private IEmailService _emailService;
        private ITimesheetService _timesheetService;

        public HolidayRequestApproveState(HolidayRequest request, ICalendarService calendarService,
            IEmailService emailService, ITimesheetService timesheetService)
        {
            context = request;
            _calendarService = calendarService;
            _emailService = emailService;
            _timesheetService = timesheetService;
        }
        public void Approve()
        {
            //Holiday Request already in Approve State.
        }

        public void Reject()
        {
            //Can we reject after a holiday has been approved?
        }
    }
}
