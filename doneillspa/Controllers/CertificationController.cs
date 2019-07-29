using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using doneillspa.Services.Certification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class CertificationController : Controller
    {
        private readonly ICertificationService _service;

        public CertificationController(ICertificationService service)
        {
            _service = service;
        }

        public bool ShouldICertify()
        {
            return true;
        }

        [HttpDelete]
        [Route("api/certification/{id}")]
        public JsonResult Delete(long id)
        {
            Certification certification = _service.GetCertificationById(id);

            if (certification != null)
            {
                _service.DeleteCertification(certification);

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}