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
    public class NotificationController : Controller
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService ns)
        {
            _service = ns;
        }

        [HttpDelete]
        [Route("api/notification/{id}")]
        public JsonResult Delete(long id)
        {
            EmailNotification not = _service.GetEmailNotificationById(id);

            if (not != null)
            {
                _service.DeleteNotification(not);

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}