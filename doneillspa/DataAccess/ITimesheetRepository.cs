using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface ITimesheetRepository
    {
        void InsertTimesheet(Timesheet b);
        void UpdateTimesheet(Timesheet b);
        IEnumerable<Timesheet> GetTimesheets();
        Timesheet GetTimsheetById(long id);
        IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting);
        void Save();
    }
}
