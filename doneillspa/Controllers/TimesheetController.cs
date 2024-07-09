using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using doneillspa.Services;
using doneillspa.Services.Email;
using doneillspa.Services.MessageQueue;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        private readonly ILogger<TimesheetController> _logger;
        private IMessageQueue _messageQueue;
        private IConfiguration _configuration;

        public TimesheetController(ILogger<TimesheetController> logger, IMapper mapper, IMediator mediator, ITimesheetRepository repository, 
            IMessageQueue messageQueue, IConfiguration configuration)
        {
            _mapper = mapper;
            _mediator = mediator;
            _timeSheetRepository = repository;
            _logger = logger;
            _messageQueue = messageQueue;
            _configuration = configuration;
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

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetSubmittedTimesheets().OrderByDescending(r => r.DateSubmitted);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/usersubmittedtimesheet")]
        public IEnumerable<TimesheetDto> GetUserSubmittedTimesheets(String userId)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetUserSubmittedTimesheets(userId).OrderByDescending(r => r.DateSubmitted);
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

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetApprovedTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/userapprovedtimesheet")]
        public IEnumerable<TimesheetDto> GetUserApprovedTimesheets(String userId)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetUserApprovedTimesheets(userId).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }


        //[HttpGet]
        //[Route("api/archievedtimesheet")]
        //public IEnumerable<TimesheetDto> GetArchievedTimesheets()
        //{
        //    List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

        //    IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetArchievedTimesheets().OrderByDescending(r => r.WeekStarting);
        //    foreach (Timesheet ts in timesheets)
        //    {
        //        timesheetsDtos.Add(ConvertToDto(ts));
        //    }
        //    return timesheetsDtos;
        //}
        [HttpPost]
        [Route("api/timesheet/report")]
        public IActionResult OrderTimesheetReport([FromBody] TimesheetReport timesheetReport)
        {
            _logger.LogInformation("About to queue report order");
            _messageQueue.SendMessage(Base64Encode(JsonConvert.SerializeObject(timesheetReport)));
            _logger.LogInformation("Finished queuing report order");
            return Ok();
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [HttpGet]
        [Route("api/archievedtimesheetforrange")]
        public IEnumerable<TimesheetDto> GetArchievedTimesheetsForRange(DateTime fromDate, DateTime toDate)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetArchievedTimesheetsForRange(fromDate, toDate).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/userarchievedtimesheetforrange")]
        public IEnumerable<TimesheetDto> GetUserArchievedTimesheetsForRange(String userId, DateTime fromDate, DateTime toDate)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetUserArchievedTimesheetsForRange(userId, fromDate, toDate).OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }


        //[HttpGet]
        //[Route("api/timesheetreport/{filename}")]
        //public Task<IActionResult> DownloadBlob(String filename)
        //{
        //    // Retrieve the connection string for use with the application. 
        //    var connectionString = _configuration["ConnectionStrings:StorageConnectionString"];

        //    // Create a BlobServiceClient object 
        //    var blobServiceClient = new BlobServiceClient(connectionString);
        //    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("doneillreports");

        //    BlobClient client = containerClient.GetBlobClient(filename);
        //    BlobDownloadResult content = client.DownloadContent();


        //}

        [HttpGet]
        [Route("api/timesheetreports")]
        public IEnumerable<ReportDto> GetReports()
        {
            List<ReportDto> reports = new List<ReportDto>();

            // Retrieve the connection string for use with the application. 
            var connectionString = _configuration["ConnectionStrings:StorageConnectionString"];

            // Create a BlobServiceClient object 
            var blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("doneillreports");

            var resultSegment = containerClient.GetBlobs().AsPages();
            foreach (Page<BlobItem> blobPage in resultSegment)
            {
                foreach (BlobItem blobItem in blobPage.Values)
                {
                    ReportDto report = new ReportDto();
                    report.Name = blobItem.Name;

                    reports.Add(report);
                }
            }
            return reports;
        }


        [HttpGet]
        [Route("api/rejectedtimesheet")]
        public IEnumerable<TimesheetDto> GetRejectedTimesheets()
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetRejectedTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }

        [HttpGet]
        [Route("api/userrejectedtimesheet")]
        public IEnumerable<TimesheetDto> GetUserRejectedTimesheets(String userId)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            IEnumerable<Timesheet> timesheets = _timeSheetRepository.GetUserRejectedTimesheets(userId).OrderByDescending(r => r.WeekStarting);
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

        private bool isProjectCode(string code)
        {
            return !(code.Equals("NC1") || code.Equals("NC2") || code.Equals("NC3") || code.Equals("NC4") || code.Equals("NC5") || code.Equals("NC6") || code.Equals("NC7"));
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
                _logger.LogWarning($"About to approve timesheet for id: {ts.Id}");
                timesheet.Approved(_mediator);
            }
            else if (ts.Status.Equals(TimesheetStatus.Rejected.ToString()))
            {
                _logger.LogWarning($"About to reject timesheet for id: {ts.Id}");
                timesheet.Rejected(_mediator);
            }
            else if (ts.Status.Equals(TimesheetStatus.Submitted.ToString()))
            {
                _logger.LogWarning($"Timesheet Submitted for id: {ts.Id}");
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
                tsedto.Chargeable = tse.Chargeable;

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