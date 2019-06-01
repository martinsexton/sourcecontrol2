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

        public IEnumerable<Timesheet> GetTimesheets()
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .ToList();
        }

        public IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting)
        {
            return _context.Timesheet
                        .Where(b => b.WeekStarting.Date == weekStarting.Date)
                        .Include(b => b.TimesheetEntries)
                        .ToList();

        }

        public IEnumerable<Timesheet> GetTimesheetsByUser(string user)
        {
            return _context.Timesheet
                .Where(b => b.Username.ToUpper().Equals(user.ToUpper()))
                .Include(b => b.TimesheetEntries)
                .ToList();
        }

        public Timesheet GetTimsheetById(long id)
        {
            return _context.Timesheet
                        .Where(b => b.Id == id)
                        .Include(b => b.TimesheetEntries)
                        .FirstOrDefault();
        }

        public long InsertTimesheet(Timesheet t)
        {
            _context.Timesheet.Add(t);
            _context.SaveChanges();
            return t.Id;
        }

        public void UpdateTimesheet(Timesheet b)
        {
            _context.Entry(b).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
