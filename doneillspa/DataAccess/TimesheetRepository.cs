using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using doneillspa.ValueObjects;
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
                .Include(b => b.TimesheetNotes)
                .ToList();
        }

        public IEnumerable<Timesheet> GetSubmittedTimesheets()
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Submitted)
                .ToList();
        }

        public IEnumerable<Timesheet> GetUserSubmittedTimesheets(String userId)
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Submitted 
                        && b.Owner.ToString().Equals(userId))
                .ToList();
        }

        public IEnumerable<Timesheet> GetApprovedTimesheets()
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Approved)
                .ToList();
        }

        public IEnumerable<Timesheet> GetUserApprovedTimesheets(String userId)
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Approved 
                        && b.Owner.ToString().Equals(userId))
                .ToList();
        }

        //public IEnumerable<Timesheet> GetArchievedTimesheets()
        //{
        //    return _context.Timesheet
        //        .Include(b => b.TimesheetEntries)
        //        .Include(b => b.TimesheetNotes)
        //        .Where(b => b.Status == TimesheetStatus.Archieved)
        //        .ToList();
        //}

        public IEnumerable<Timesheet> GetArchievedTimesheetsForRange(DateTime fromDate, DateTime toDate)
        {
            return _context.Timesheet
                        .Where(b => b.WeekStarting.Date >= fromDate.Date 
                                && b.WeekStarting.Date <= toDate.Date 
                                && b.Status == TimesheetStatus.Archieved)
                        .Include(b => b.TimesheetEntries)
                        .Include(b => b.TimesheetNotes)
                        .ToList();
        }

        public IEnumerable<Timesheet> GetUserArchievedTimesheetsForRange(string userId, DateTime fromDate, DateTime toDate)
        {
            return _context.Timesheet
                        .Where(b => b.WeekStarting.Date >= fromDate.Date
                                && b.WeekStarting.Date <= toDate.Date
                                && b.Status == TimesheetStatus.Archieved
                                && b.Owner.ToString().Equals(userId))
                        .Include(b => b.TimesheetEntries)
                        .Include(b => b.TimesheetNotes)
                        .ToList();
        }

        public IEnumerable<Timesheet> GetRejectedTimesheets()
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Rejected)
                .ToList();
        }

        public IEnumerable<Timesheet> GetUserRejectedTimesheets(string userId)
        {
            return _context.Timesheet
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .Where(b => b.Status == TimesheetStatus.Rejected
                        && b.Owner.ToString().Equals(userId))
                .ToList();
        }



        public IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting)
        {
            return _context.Timesheet
                        .Where(b => b.WeekStarting.Date == weekStarting.Date)
                        .Include(b => b.TimesheetEntries)
                        .Include(b => b.TimesheetNotes)
                        .ToList();

        }

        public double GetRateForTimesheet(Timesheet ts)
        {
            List<LabourRate> rates = _context.LabourRate.ToList();

            LabourRate rate = rates.Where(r => r.Role.Equals(ts.Role) && r.EffectiveFrom <= ts.WeekStarting &&
                    (r.EffectiveTo == null || r.EffectiveTo >= ts.WeekStarting)).FirstOrDefault();

            if (rate != null)
            {
                return rate.RatePerHour;
            }

            return 0.0;
        }

        public IEnumerable<Timesheet> GetTimesheetsByUserId(string userId)
        {
            return _context.Timesheet
                .Where(b => b.Owner.ToString() == userId)
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .ToList();
        }

        public IEnumerable<Timesheet> GetTimesheetsByUserAndDate(string user, DateTime weekStarting)
        {
            return _context.Timesheet
                .Where(b => b.Username.ToUpper().Equals(user.ToUpper()) && b.WeekStarting.Date == weekStarting.Date)
                .Include(b => b.TimesheetEntries)
                .Include(b => b.TimesheetNotes)
                .ToList();
        }

        public Timesheet GetTimsheetById(long id)
        {
            return _context.Timesheet
                        .Where(b => b.Id == id)
                        .Include(b => b.TimesheetEntries)
                        .Include(b => b.TimesheetNotes)
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

        public IList<long> GetRelevantTimesheets(string proj)
        {
            IList<long> timesheetIds = new List<long>();
            IEnumerable<TimesheetEntry> timesheetEntries;

            timesheetEntries = _context.TimesheetEntry
                .Include(r => r.Timesheet)
                .Where(r => r.Code.Equals(proj));
    
            foreach (TimesheetEntry tse in timesheetEntries)
            {
                if (!timesheetIds.Contains(tse.Timesheet.Id))
                {
                        timesheetIds.Add(tse.Timesheet.Id);
                }
            }
            return timesheetIds;
        }
    }
}
