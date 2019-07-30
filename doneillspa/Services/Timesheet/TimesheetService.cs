using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
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

        public void DeleteNote(TimesheetNote note)
        {
            _noteRepository.DeleteNote(note);
        }

        public void DeleteTimesheetEntry(TimesheetEntry tse)
        {
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
                        timesheet = new Timesheet();
                        timesheet.DateCreated = DateTime.UtcNow;
                        timesheet.Username = userName;
                        timesheet.WeekStarting = startOfWeek;
                        timesheet.Role = role;
                        timesheet.Owner = userToVerify.Id;
                        timesheet.TimesheetEntries = new List<TimesheetEntry>();
                        timesheet.Status = TimesheetStatus.New;

                        _repository.InsertTimesheet(timesheet);
                    }
                    if (!timesheets.Contains(timesheet))
                    {
                        timesheets.Add(timesheet);
                    }
                }
                if(timesheet != null)
                {
                    if (!start.DayOfWeek.ToString().Equals("Sunday"))
                    {
                        TimesheetEntry tse = new TimesheetEntry();
                        tse.StartTime = "09:00";
                        tse.EndTime = "17:30";

                        //Anual leave is code NC1
                        tse.Code = "NC1";
                        tse.DateCreated = DateTime.UtcNow;

                        if (start.DayOfWeek.ToString().Equals("Monday"))
                        {
                            tse.Day = "Mon";
                        }
                        else if (start.DayOfWeek.ToString().Equals("Tuesday"))
                        {
                            tse.Day = "Tue";
                        }
                        else if (start.DayOfWeek.ToString().Equals("Wednesday"))
                        {
                            tse.Day = "Wed";
                        }
                        else if (start.DayOfWeek.ToString().Equals("Thursday"))
                        {
                            tse.Day = "Thurs";
                        }
                        else if (start.DayOfWeek.ToString().Equals("Friday"))
                        {
                            tse.Day = "Fri";
                        }
                        else
                        {
                            tse.Day = "Sat";
                        }
                        timesheet.AddTimesheetEntry(tse);
                    }
                    //Increment date
                    start = start.AddDays(1);
                }
            }

            //Update timesheets with all entries added
            foreach(Timesheet ts in timesheets)
            {
                _repository.UpdateTimesheet(ts);
            }
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
