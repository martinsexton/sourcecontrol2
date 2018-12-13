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
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _repository;

        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpDelete]
        [Route("api/notification/{id}")]
        public JsonResult Delete(long id)
        {
            EmailNotification not = _repository.GetEmailNotificationById(id);

            if (not != null)
            {
                _repository.DeleteCertification(not);
                _repository.Save();

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}