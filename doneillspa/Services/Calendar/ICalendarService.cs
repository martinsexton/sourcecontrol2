using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services.Calendar
{
    public interface ICalendarService
    {
        void CreateEvent(DateTime fromDate, int days, string description);
    }
}
