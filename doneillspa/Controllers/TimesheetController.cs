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
    //[Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    [Route("api/timesheet")]
    public class TimesheetController : Controller
    {
        private readonly ITimesheetRepository _repository;

        public TimesheetController(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Timesheet> Get()
        {
            return _repository.GetTimesheets();
        }

        [HttpGet("{id}")]
        public JsonResult Get(long id)
        {
            var item = _repository.GetTimsheetById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Timesheet timesheet)
        {
            if (timesheet == null)
            {
                return BadRequest();
            }

            _repository.InsertTimesheet(timesheet);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Timesheet t)
        {
            if (t == null || t.Id != id)
            {
                return BadRequest();
            }

            var existingTimesheet = _repository.GetTimsheetById(t.Id);

            if (existingTimesheet == null)
            {
                return NotFound();
            }

            existingTimesheet.Owner = t.Owner;
            existingTimesheet.WeekStarting = t.WeekStarting;

            _repository.UpdateTimesheet(existingTimesheet);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return new NoContentResult();
        }
    }
}