using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface ITimesheetService
    {
        long InsertTimesheet(Timesheet b);
        void UpdateTimesheet(Timesheet b);
        IEnumerable<Timesheet> GetTimesheets();
        Timesheet GetTimsheetById(long id);
        IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting);
        IEnumerable<Timesheet> GetTimesheetsByUser(string user);
        IEnumerable<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime weekStarting);
        TimesheetEntry GetTimsheetEntryById(long id);
        void UpdateTimesheetEntry(TimesheetEntry tse);

        void RecordAnnualLeave(string userId, DateTime start, int numberOfDays);
        LabourWeekDetail BuildLabourWeekDetails(Timesheet ts, List<LabourRate> Rates, string proj);


    }
}
