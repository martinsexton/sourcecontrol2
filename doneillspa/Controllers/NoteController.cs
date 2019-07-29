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
    public class NoteController : Controller
    {
        private readonly ITimesheetService _timesheetService;

        public NoteController(ITimesheetService tss)
        {
            _timesheetService = tss;
        }

        [HttpDelete]
        [Route("api/note/{id}")]
        public JsonResult Delete(long id)
        {
            TimesheetNote note = _timesheetService.GetNoteById(id);

            if (note != null)
            {
                _timesheetService.DeleteNote(note);

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}