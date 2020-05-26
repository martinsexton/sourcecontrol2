using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface ITimesheetService
    {
        void RecordAnnualLeave(string userId, DateTime start, int numberOfDays);
        LabourWeekDetail BuildLabourWeekDetails(Timesheet ts, List<LabourRate> Rates, string proj);


    }
}
