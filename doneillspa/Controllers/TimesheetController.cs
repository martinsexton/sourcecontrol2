using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using doneillspa.Services;
using doneillspa.Services.Email;
using hub;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TimesheetController(IMapper mapper, IMediator mediator, ITimesheetRepository repository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _timeSheetRepository = repository;
        }

        [HttpGet]
        [Route("api/timesheet")]
        public IEnumerable<TimesheetDto> Get()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach(Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/submittedtimesheet")]
        public IEnumerable<TimesheetDto> GetSubmittedTimesheets()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheets().Where(t =>t.Status == TimesheetStatus.Submitted).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/approvedtimesheet")]
        public IEnumerable<TimesheetDto> GetApprovedTimesheets()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheets().Where(t => t.Status == TimesheetStatus.Approved && t.WeekStarting >= DateTime.Now.AddDays(-30)).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }


        [HttpGet]
        [Route("api/archievedtimesheet")]
        public IEnumerable<TimesheetDto> GetArchievedTimesheets()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheets().Where(t => t.Status == TimesheetStatus.Approved && t.WeekStarting < DateTime.Now.AddDays(-30)).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }


        [HttpGet]
        [Route("api/rejectedtimesheet")]
        public IEnumerable<TimesheetDto> GetRejectedTimesheets()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheets().Where(t => t.Status == TimesheetStatus.Rejected).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/timesheet/{id}")]
        public JsonResult Get(long id)
        {
            return new JsonResult(ConvertToDto(_timeSheetRepository.GetTimsheetById(id)));
        }

        [HttpGet]
        [Route("api/projectassignments/week/{year}/{month}/{day}")]
        public IEnumerable<ProjectAssignmentDto> Get(int year, int month, int day)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();
            IEnumerable<Timesheet> timesheets = new List<Timesheet>();
            IList<ProjectAssignmentDto> assignments = new List<ProjectAssignmentDto>();

            //If no date provide, then bring back all timesheets
            if (year == 0 || year == 1970)
            {
                timesheets = _timeSheetRepository.GetTimesheets();
            }
            else
            {
                DateTime weekStarting = new DateTime(year, month, day);
                timesheets = _timeSheetRepository.GetTimesheetsByDate(weekStarting);
            }

            foreach (Timesheet ts in timesheets)
            {
                //We only are concerned with approved timesheets
                if (ts.Status != TimesheetStatus.Approved) continue;

                foreach (TimesheetEntry tse in ts.TimesheetEntries)
                {
                    if (!projectCodeAlreadyRecorded(assignments, tse.Code))
                    {
                        //For none chargeable codes we ignore.
                        if (!isProjectCode(tse.Code))
                        {
                            continue;
                        }
                        ProjectAssignmentDto adto = new ProjectAssignmentDto();
                        adto.Code = tse.Code;
                        if (adto.Users == null)
                        {
                            adto.Users = new List<UserAssignmentDetails>();
                        }
                        bool addUser = true;
                        foreach (UserAssignmentDetails user in adto.Users)
                        {
                            if (user.UserName.Equals(ts.Username))
                            {
                                addUser = false;
                            }
                        }
                        if (addUser)
                        {
                            UserAssignmentDetails newUserDetails = new UserAssignmentDetails();
                            newUserDetails.UserName = ts.Username;
                            DateTime now = DateTime.UtcNow;
                            string[] startTime = tse.StartTime.Split(":");
                            string[] endTime = tse.EndTime.Split(":");

                            DateTime start = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(startTime[0]), Int32.Parse(startTime[1]), 0);
                            DateTime end = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(endTime[0]), Int32.Parse(endTime[1]), 0);

                            TimeSpan varTime = (DateTime)end - (DateTime)start;

                            newUserDetails.TotalMinutes = (int)varTime.TotalMinutes;
                            adto.Users.Add(newUserDetails);
                        }
                        assignments.Add(adto);
                    }
                    else
                    {
                        foreach (ProjectAssignmentDto dto in assignments)
                        {
                            if (dto.Code.Equals(tse.Code))
                            {
                                if (dto.Users == null)
                                {
                                    dto.Users = new List<UserAssignmentDetails>();
                                }
                                bool addUser = true;
                                foreach (UserAssignmentDetails user in dto.Users)
                                {
                                    if (user.UserName.Equals(ts.Username))
                                    {
                                        addUser = false;
                                    }
                                }
                                if (addUser)
                                {
                                    UserAssignmentDetails newUserDetails = new UserAssignmentDetails();
                                    newUserDetails.UserName = ts.Username;
                                    DateTime now = DateTime.UtcNow;
                                    string[] startTime = tse.StartTime.Split(":");
                                    string[] endTime = tse.EndTime.Split(":");

                                    DateTime start = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(startTime[0]), Int32.Parse(startTime[1]), 0);
                                    DateTime end = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(endTime[0]), Int32.Parse(endTime[1]), 0);

                                    TimeSpan varTime = (DateTime)end - (DateTime)start;

                                    newUserDetails.TotalMinutes = (int)varTime.TotalMinutes;
                                    dto.Users.Add(newUserDetails);
                                }
                                else
                                {
                                    foreach (UserAssignmentDetails user in dto.Users)
                                    {
                                        if (user.UserName.Equals(ts.Username))
                                        {
                                            DateTime now = DateTime.UtcNow;
                                            string[] startTime = tse.StartTime.Split(":");
                                            string[] endTime = tse.EndTime.Split(":");

                                            DateTime start = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(startTime[0]), Int32.Parse(startTime[1]), 0);
                                            DateTime end = new DateTime(now.Year, now.Month, now.Day, Int32.Parse(endTime[0]), Int32.Parse(endTime[1]), 0);

                                            TimeSpan varTime = (DateTime)end - (DateTime)start;

                                            user.TotalMinutes += (int)varTime.TotalMinutes;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return assignments;

        }

        private bool isProjectCode(string code)
        {
            return !(code.Equals("NC1") || code.Equals("NC2") || code.Equals("NC3") || code.Equals("NC4") || code.Equals("NC5") || code.Equals("NC6") || code.Equals("NC7"));
        }

        private bool projectCodeAlreadyRecorded(IList<ProjectAssignmentDto> assignments, string code)
        {
            bool projectAlreadyAdded = false;
            foreach (ProjectAssignmentDto dto in assignments)
            {
                if (dto.Code.Equals(code))
                {
                    projectAlreadyAdded = true;
                }
            }
            return projectAlreadyAdded;
        }

        [HttpGet]
        [Route("api/timesheet/name/{user}/week/{year}/{month}/{day}")]
        public IEnumerable<TimesheetDto> Get(string user, int year, int month, int day)
        {
            DateTime weekStarting = new DateTime(year, month, day);

            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetTimesheetsByUserAndDate(user, weekStarting);
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
            return Ok(_timeSheetRepository.InsertTimesheet(timesheet));
        }

        [HttpPut()]
        [Route("api/timesheet")]
        public IActionResult Put(int id, [FromBody]TimesheetDto ts)
        {
            Timesheet timesheet = _timeSheetRepository.GetTimsheetById(ts.Id);
            if (ts.Status.Equals(TimesheetStatus.Approved.ToString()))
            {
                timesheet.Approved(_mediator);
            }
            else if (ts.Status.Equals(TimesheetStatus.Rejected.ToString()))
            {
                timesheet.Rejected(_mediator);
            }
            else if (ts.Status.Equals(TimesheetStatus.Submitted.ToString()))
            {
                timesheet.Submitted(_mediator);
            }

            _timeSheetRepository.UpdateTimesheet(timesheet);

            return Ok();
        }


        [HttpPut()]
        [Route("api/timesheet/{id}/timesheetentry")]
        public IActionResult Put(int id, [FromBody]TimesheetEntry entry)
        {
            var existingTimesheet = _timeSheetRepository.GetTimsheetById(id);

            if (existingTimesheet.CanAddTimesheetEntry(entry))
            {
                existingTimesheet.AddTimesheetEntry(entry);

                _timeSheetRepository.UpdateTimesheet(existingTimesheet);

                return Ok(entry.Id);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPut()]
        [Route("api/timesheet/{id}/note")]
        public IActionResult Put(int id, [FromBody]TimesheetNote note)
        {
            if(note != null)
            {
                var existingTimesheet = _timeSheetRepository.GetTimsheetById(id);
                existingTimesheet.AddTimesheetNote(_mediator, note);

                _timeSheetRepository.UpdateTimesheet(existingTimesheet);

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
            if (ts.DateSubmitted.HasValue)
            {
                tsdto.DateSubmitted = ts.DateSubmitted.Value;
            }
            

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
                tsedto.Username = tse.Timesheet.Username;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            foreach(TimesheetNote note in ts.TimesheetNotes)
            {
                tsdto.TimesheetNotes.Add(_mapper.Map<TimesheetNoteDto>(note));
            }

            return tsdto;
        }

    }
}