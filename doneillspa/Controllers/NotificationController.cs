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
    public class NotificationController : Controller
    {
        private ApplicationContext _context;

        public NotificationController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpDelete]
        [Route("api/notification/{id}")]
        public JsonResult Delete(long id)
        {
            EmailNotification notification = _context.EmailNotification
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

            _context.Entry(notification).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }
    }
}