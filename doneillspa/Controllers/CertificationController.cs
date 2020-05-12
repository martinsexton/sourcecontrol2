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
    public class CertificationController : Controller
    {
        ApplicationContext _context;

        public CertificationController(ApplicationContext context)
        {
            _context = context;
        }

        public bool ShouldICertify()
        {
            return true;
        }

        [HttpDelete]
        [Route("api/certification/{id}")]
        public JsonResult Delete(long id)
        {
            Certification cert = _context.Certification
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

            _context.Entry(cert).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }
    }
}