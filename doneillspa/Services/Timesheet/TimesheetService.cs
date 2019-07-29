using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repository;
        private readonly ITimesheetEntryRepository _tseRepository;
        private readonly INoteRepository _noteRepository;

        public TimesheetService(ITimesheetRepository tsr, ITimesheetEntryRepository tser, INoteRepository nr)
        {
            _repository = tsr;
            _tseRepository = tser;
            _noteRepository = nr;
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

        public void InsertTimesheetEntry(TimesheetEntry tse)
        {
            _tseRepository.InsertTimesheetEntry(tse);
            _tseRepository.Save();
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
