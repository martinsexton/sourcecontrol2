using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Calendar;

namespace doneillspa.Models.State
{
    public interface IHolidayRequestState
    {
        void Approve(ICalendarService _calendarService);
        void Reject();
    }
}
