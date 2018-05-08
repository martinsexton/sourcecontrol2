using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface ITimesheetEntryRepository
    {
        TimesheetEntry GetTimsheetEntryById(long id);
        void InsertTimesheetEntry(TimesheetEntry tse);
        void DeleteTimesheetEntry(TimesheetEntry tse);
        void Save();
    }
}
