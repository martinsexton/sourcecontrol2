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
    public class NoteController : Controller
    {
        private readonly INoteRepository _repository;

        public NoteController(INoteRepository repository)
        {
            _repository = repository;
        }

        [HttpDelete]
        [Route("api/note/{id}")]
        public JsonResult Delete(long id)
        {
            TimesheetNote note = _repository.GetNoteById(id);

            if (note != null)
            {
                _repository.DeleteNote(note);

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}