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
    public class NoteController : Controller
    {
        private ApplicationContext _context;

        public NoteController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpDelete]
        [Route("api/note/{id}")]
        public JsonResult Delete(long id) {

            TimesheetNote note =  _context.TimesheetNote
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

            _context.Entry(note).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }
    }
}