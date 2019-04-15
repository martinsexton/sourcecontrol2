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
        void Created(IEmailService _emailServie);
        void Approve(ICalendarService _calendarService);
        void Reject();
    }
}
