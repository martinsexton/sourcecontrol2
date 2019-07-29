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

        TimesheetEntry GetTimsheetEntryById(long id);
        void InsertTimesheetEntry(TimesheetEntry tse);
        void UpdateTimesheetEntry(TimesheetEntry tse);
        void DeleteTimesheetEntry(TimesheetEntry tse);
    }
}
