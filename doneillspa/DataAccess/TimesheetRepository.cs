using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationContext _context;

        public TimesheetRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Timesheet GetTimesheetByUserAndDate(Guid userId, DateTime weekStarting)
        {
            return _context.Timesheet
                        .Where(b => b.Owner == userId && b.WeekStarting == weekStarting)
                        .Include(b => b.TimesheetEntries)
                        .FirstOrDefault();
        }

        public IEnumerable<Timesheet> GetTimesheets()
        {
            return _context.Timesheet.ToList();
        }

        public Timesheet GetTimsheetById(long id)
        {
            return _context.Timesheet
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }

        public void InsertTimesheet(Timesheet t)
        {
            _context.Timesheet.Add(t);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateTimesheet(Timesheet b)
        {
            _context.Entry(b).State = EntityState.Modified;
        }
    }
}
