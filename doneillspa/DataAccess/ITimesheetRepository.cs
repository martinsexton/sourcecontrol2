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
        IEnumerable<Timesheet> GetSubmittedTimesheets();
        IEnumerable<Timesheet> GetApprovedTimesheets();
        IEnumerable<Timesheet> GetArchievedTimesheets();
        IEnumerable<Timesheet> GetArchievedTimesheetsForRange(DateTime fromDate, DateTime toDate);
        IEnumerable<Timesheet> GetRejectedTimesheets();
        IList<long> GetRelevantTimesheets(string proj);


        Timesheet GetTimsheetById(long id);
        IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting);
        IEnumerable<Timesheet> GetTimesheetsByUserId(string userId);

        IEnumerable<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime weekStarting);
        double GetRateForTimesheet(Timesheet ts);
    }
}
