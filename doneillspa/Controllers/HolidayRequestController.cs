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
        private readonly IHolidayService _holidayService;
        private readonly ITimesheetService _timesheetService;

        public HolidayRequestController(IEmailService emailService, ICalendarService calendarService, IHolidayService hservice, ITimesheetService tss)
        {
            _emailService = emailService;
            _calendarService = calendarService;
            _holidayService = hservice;
            _timesheetService = tss;
        }
        [HttpPut]
        [Route("api/holidayrequest")]
        public IActionResult Put([FromBody]HolidayRequestDto hr)
        {
            HolidayRequest request = _holidayService.GetHolidayRequestById(hr.Id);
            request.Updated(hr, _calendarService, _emailService, _timesheetService);

            _holidayService.Update(request);
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("api/holidayrequest/{id}")]
        public JsonResult Delete(long id)
        {
            _holidayService.Delete(id);
            return Json(Ok());
        }
    }
}