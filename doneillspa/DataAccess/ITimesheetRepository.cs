using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface ITimesheetRepository
    {
        long InsertTimesheet(Timesheet b);
        void UpdateTimesheet(Timesheet b);
        IEnumerable<Timesheet> GetTimesheets();
        Timesheet GetTimsheetById(long id);
        IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting);
        IEnumerable<Timesheet> GetTimesheetsByUser(string user);
        IEnumerable<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime weekStarting);
        double GetRateForTimesheet(Timesheet ts);
    }
}
