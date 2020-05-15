using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using doneillspa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TimesheetEntryController : Controller
    {
        private ApplicationContext _context;

        public TimesheetEntryController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Get(long id)
        {
            TimesheetEntry item = _context.TimesheetEntry
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

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

            TimesheetEntry item = _context.TimesheetEntry
            .Where(b => b.Id == tse.Id)
            .FirstOrDefault();

            item.StartTime = tse.StartTime;
            item.Details = tse.Details;
            item.EndTime = tse.EndTime;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("api/timesheetentry/{id}")]
        public JsonResult Delete(long id)
        {
            TimesheetEntry item = _context.TimesheetEntry
                .Where(b => b.Id == id)
                .FirstOrDefault();

            _context.Entry(item).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }
    }
}