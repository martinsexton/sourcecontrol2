using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using doneillspa.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class TimesheetEntryRepository : ITimesheetEntryRepository
    {
        private readonly ApplicationContext _context;

        public TimesheetEntryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void DeleteTimesheetEntry(TimesheetEntry tse)
        {
            _context.Entry(tse).State = EntityState.Deleted;
        }

        public TimesheetEntry GetTimsheetEntryById(long id)
        {
            return _context.TimesheetEntry
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }

        public void InsertTimesheetEntry(TimesheetEntry tse)
        {
            _context.TimesheetEntry.Add(tse);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateTimesheetEntry(TimesheetEntry tse)
        {
            _context.Entry(tse).State = EntityState.Modified;
        }
    }
}
