using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

namespace doneillspa.Models.State
{
    public interface IHolidayRequestState
    {
        void Created(IEmailService _emailService);
        void Approve(ICalendarService _calendarService, IEmailService _emailService);
        void Reject(IEmailService _emailService);
    }
}
