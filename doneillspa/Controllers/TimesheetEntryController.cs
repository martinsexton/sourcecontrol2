using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using doneillspa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetEntryController : Controller
    {
        private readonly ITimesheetService _timesheetService;

        public TimesheetEntryController(ITimesheetService tss)
        {
            _timesheetService = tss;
        }

        [HttpGet]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Get(long id)
        {
            var item = _timesheetService.GetTimsheetEntryById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }

        [HttpPut]
        [Route("api/timesheetentry")]
        public IActionResult Put([FromBody]TimesheetEntry tse)
        {
            if (tse == null)
            {
                return BadRequest();
            }

            _timesheetService.UpdateTimesheetEntry(tse);

            return Ok();
        }

        [HttpDelete]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Delete(long id)
        {
            TimesheetEntry timesheetentry = _timesheetService.GetTimsheetEntryById(id);
           
            if (timesheetentry != null)
            {
                _timesheetService.DeleteTimesheetEntry(timesheetentry);

                return Json(Ok());
            }

            return Json(Ok());
        }
    }
}