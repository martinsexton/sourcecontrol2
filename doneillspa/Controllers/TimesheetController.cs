﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using doneillspa.Services;
using doneillspa.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetService _service;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetController(UserManager<ApplicationUser> userManager, ITimesheetService tss, IEmailService emailService)
        {
            _service = tss;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/timesheet")]
        public IEnumerable<TimesheetDto> Get()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _service.GetTimesheets().OrderByDescending(r => r.WeekStarting);
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
            Timesheet item = _service.GetTimsheetById(id);

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
                timesheets = _service.GetTimesheets();
            }
            else
            {
                DateTime weekStarting = new DateTime(year, month, day);
                timesheets = _service.GetTimesheetsByDate(weekStarting);
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

            IEnumerable<Timesheet> timesheets = _service.GetTimesheetsByUser(user);
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

            long id = _service.InsertTimesheet(timesheet);

            return Ok(id);
        }

        [HttpPut()]
        [Route("api/timesheet")]
        public IActionResult Put(int id, [FromBody]TimesheetDto ts)
        {
            Timesheet timesheet = _service.GetTimsheetById(ts.Id);
            timesheet.Updated(ts);

            _service.UpdateTimesheet(timesheet);

            return Ok();
        }


        [HttpPut()]
        [Route("api/timesheet/{id}/timesheetentry")]
        public IActionResult Put(int id, [FromBody]TimesheetEntry entry)
        {
            var existingTimesheet = _service.GetTimsheetById(id);
            existingTimesheet.AddTimesheetEntry(entry);

            _service.UpdateTimesheet(existingTimesheet);

            return Ok(entry.Id);
        }

        [HttpPut()]
        [Route("api/timesheet/{id}/note")]
        public IActionResult Put(int id, [FromBody]TimesheetNote note)
        {
            if(note != null)
            {
                var existingTimesheet = _service.GetTimsheetById(id);
                existingTimesheet.AddTimesheetNote(_userManager, _emailService, note);

                _service.UpdateTimesheet(existingTimesheet);

                return Ok(note.Id);
            }
            return BadRequest();
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
            tsdto.TimesheetNotes = new List<TimesheetNoteDto>();

            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                TimesheetEntryDto tsedto = new TimesheetEntryDto();
                tsedto.DateCreated = tse.DateCreated;
                tsedto.Day = tse.Day;
                tsedto.Details = tse.Details;
                tsedto.EndTime = tse.EndTime;
                tsedto.Id = tse.Id;
                tsedto.Code = tse.Code;
                tsedto.StartTime = tse.StartTime;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            foreach(TimesheetNote note in ts.TimesheetNotes)
            {
                TimesheetNoteDto ndto = new TimesheetNoteDto();
                ndto.Id = note.Id;
                ndto.Details = note.Details;

                tsdto.TimesheetNotes.Add(ndto);
            }

            return tsdto;
        }

    }
}