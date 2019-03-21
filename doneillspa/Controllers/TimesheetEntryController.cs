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
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetEntryController : Controller
    {
        private readonly ITimesheetEntryRepository _repository;

        public TimesheetEntryController(ITimesheetEntryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Get(long id)
        {
            var item = _repository.GetTimsheetEntryById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }

        [HttpPost]
        [Route("api/timesheetentry")]
        public IActionResult Post([FromBody]TimesheetEntry tse)
        {
            if (tse == null)
            {
                return BadRequest();
            }

            //Set date created on timesheet entry
            tse.DateCreated = DateTime.UtcNow;

            _repository.InsertTimesheetEntry(tse);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return Ok();
        }

        [HttpDelete]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Delete(long id)
        {
            TimesheetEntry timesheetentry = _repository.GetTimsheetEntryById(id);
           
            if (timesheetentry != null)
            {
                _repository.DeleteTimesheetEntry(timesheetentry);
                _repository.Save();

                return Json(Ok());
            }

            return Json(Ok());
        }
    }
}