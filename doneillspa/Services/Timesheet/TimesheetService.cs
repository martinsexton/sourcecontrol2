using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Factories;
using doneillspa.Helpers;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repository;
        private readonly ITimesheetEntryRepository _tseRepository;
        private readonly INoteRepository _noteRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetService(ITimesheetRepository tsr, ITimesheetEntryRepository tser, INoteRepository nr, UserManager<ApplicationUser> userManager)
        {
            _repository = tsr;
            _tseRepository = tser;
            _noteRepository = nr;
            _userManager = userManager;
        }

        public void DeleteNote(long noteId)
        {
            TimesheetNote note = _noteRepository.GetNoteById(noteId);
            _noteRepository.DeleteNote(note);
        }

        public void DeleteTimesheetEntry(long tseId)
        {
            TimesheetEntry tse = _tseRepository.GetTimsheetEntryById(tseId);
            _tseRepository.DeleteTimesheetEntry(tse);
            _tseRepository.Save();
        }

        public TimesheetNote GetNoteById(long id)
        {
            return _noteRepository.GetNoteById(id);
        }

        public IEnumerable<Timesheet> GetTimesheets()
        {
            return _repository.GetTimesheets();
        }

        public IEnumerable<Timesheet> GetTimesheetsByDate(DateTime weekStarting)
        {
            return _repository.GetTimesheetsByDate(weekStarting);
        }

        public IEnumerable<Timesheet> GetTimesheetsByUser(string user)
        {
            return _repository.GetTimesheetsByUser(user);
        }

        public Timesheet GetTimsheetById(long id)
        {
            return _repository.GetTimsheetById(id);
        }

        public TimesheetEntry GetTimsheetEntryById(long id)
        {
            return _tseRepository.GetTimsheetEntryById(id);
        }

        public long InsertTimesheet(Timesheet b)
        {
            return _repository.InsertTimesheet(b);
        }

        public void RecordAnnualLeave(string userName, DateTime start, int numberOfDays)
        {
            ApplicationUser userToVerify = _userManager.FindByNameAsync(userName.ToUpper()).Result;
            IList<string> roles = _userManager.GetRolesAsync(userToVerify).Result;

            string role = roles.FirstOrDefault();

            List<DateTime> startOfWeeks = new List<DateTime>();
            List<Timesheet> timesheets = new List<Timesheet>();
            Timesheet timesheet = null;

            for (int i = 0; i < numberOfDays; i++)
            {
                DateTime startOfWeek = GetFirstDayOfWeek(start);
                if (!startOfWeeks.Contains(startOfWeek)){
                    startOfWeeks.Add(startOfWeek);
                    timesheet = _repository.GetTimesheetsByUser(userName).Where(ts => ts.WeekStarting.Date.Equals(startOfWeek.Date)).FirstOrDefault();
                    if(timesheet == null)
                    {
                        timesheet = TimesheetFactory.CreateTimesheet(userName, startOfWeek, role, userToVerify.Id);
                        _repository.InsertTimesheet(timesheet);
                    }
                    if (!timesheets.Contains(timesheet))
                    {
                        timesheets.Add(timesheet);
                    }
                }
                if(timesheet != null)
                {
                    if (!start.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        timesheet.AddTimesheetEntry(CreateEntryForDate(start));
                    }
                    //Increment date
                    start = start.AddDays(1);
                }
            }

            SaveTimesheetEntries(timesheets);
        }

        private void SaveTimesheetEntries(List<Timesheet> timesheets)
        {
            foreach (Timesheet ts in timesheets)
            {
                _repository.UpdateTimesheet(ts);
            }
        }

        private TimesheetEntry CreateEntryForDate(DateTime date)
        {
            return TimesheetFactory.CreateFullDayEntryForDay(Constants.Strings.Timesheets.NonChargeableCodes.AnnualLeave, date.DayOfWeek);
        }

        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != DayOfWeek.Monday)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        public void UpdateTimesheet(Timesheet b)
        {
            _repository.UpdateTimesheet(b);
        }

        public void UpdateTimesheetEntry(TimesheetEntry tse)
        {
            _tseRepository.UpdateTimesheetEntry(tse);
            _tseRepository.Save();
        }
    }
}
