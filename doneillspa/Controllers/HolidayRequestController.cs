using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using doneillspa.Services.Email;
using doneillspa.Dtos;
using doneillspa.Services.Calendar;
using doneillspa.Services;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class HolidayRequestController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ICalendarService _calendarService;
        private readonly ITimesheetService _timesheetService;
        private ApplicationContext _context;

        public HolidayRequestController(IEmailService emailService, ICalendarService calendarService, ITimesheetService tss, ApplicationContext context)
        {
            _emailService = emailService;
            _calendarService = calendarService;
            _timesheetService = tss;
            _context = context;
        }
        [HttpPut]
        [Route("api/holidayrequest")]
        public IActionResult Put([FromBody]HolidayRequestDto hr)
        {
            HolidayRequest request = _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Include(b => b.User)
                        .Where(b => b.Id == hr.Id)
                        .FirstOrDefault();

            request.Updated(hr, _calendarService, _emailService, _timesheetService);

            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("api/holidayrequest/{id}")]
        public JsonResult Delete(long id)
        {
            HolidayRequest request = _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Include(b => b.User)
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

            _context.Entry(request).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }
    }
}