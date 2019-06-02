using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _repository;

        public TimesheetController(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/timesheet")]
        public IEnumerable<TimesheetDto> Get()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach(Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/timesheet/{id}")]
        public JsonResult Get(long id)
        {
            Timesheet item = _repository.GetTimsheetById(id);

            if (item == null)
            {
                return Json(Ok());
            }

            JsonResult result = new JsonResult(ConvertToDto(item));
            return result;
        }

        [HttpGet]
        [Route("api/timesheet/week/{year}/{month}/{day}")]
        public IEnumerable<TimesheetDto> Get(int year, int month, int day)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();
            IEnumerable<Timesheet> timesheets = new List<Timesheet>();

            //If no date provide, then bring back all timesheets
            if (year == 0 || year == 1970)
            {
                timesheets = _repository.GetTimesheets();
            }
            else
            {
                DateTime weekStarting = new DateTime(year, month, day);
                timesheets = _repository.GetTimesheetsByDate(weekStarting);
            }

            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;

        }

        [HttpGet]
        [Route("api/timesheet/name/{user}")]
        public IEnumerable<TimesheetDto> Get(string user)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheetsByUser(user);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpPost]
        [Route("api/timesheet")]
        public IActionResult Post([FromBody]Timesheet timesheet)
        {
            timesheet.OnCreation();

            long id = _repository.InsertTimesheet(timesheet);

            return Ok(id);
        }

        [HttpPut()]
        [Route("api/timesheet")]
        public IActionResult Put(int id, [FromBody]TimesheetDto ts)
        {
            Timesheet timesheet = _repository.GetTimsheetById(ts.Id);
            timesheet.Updated(ts);

            _repository.UpdateTimesheet(timesheet);

            return Ok();
        }


        [HttpPut()]
        [Route("api/timesheet/{id}")]
        public IActionResult Put(int id, [FromBody]TimesheetEntry entry)
        {
            var existingTimesheet = _repository.GetTimsheetById(id);
            existingTimesheet.AddTimesheetEntry(entry);

            _repository.UpdateTimesheet(existingTimesheet);

            return Ok(entry.Id);
        }

        private TimesheetDto ConvertToDto(Timesheet ts)
        {
            TimesheetDto tsdto = new TimesheetDto();
            tsdto.DateCreated = ts.DateCreated;
            tsdto.Id = ts.Id;
            tsdto.Owner = ts.Owner;
            tsdto.Role = ts.Role;
            tsdto.Username = ts.Username;
            tsdto.WeekStarting = ts.WeekStarting;
            tsdto.Status = ts.Status.ToString();

            tsdto.TimesheetEntries = new List<TimesheetEntryDto>();
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                TimesheetEntryDto tsedto = new TimesheetEntryDto();
                tsedto.DateCreated = tse.DateCreated;
                tsedto.Day = tse.Day;
                tsedto.Details = tse.Details;
                tsedto.EndTime = tse.EndTime;
                tsedto.Id = tse.Id;
                tsedto.Project = tse.Project;
                tsedto.StartTime = tse.StartTime;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            return tsdto;
        }

    }
}