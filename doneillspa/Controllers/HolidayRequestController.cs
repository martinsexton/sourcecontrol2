using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using doneillspa.Services.Email;
using doneillspa.Dtos;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class HolidayRequestController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IHolidayRequestRepository _repository;

        public HolidayRequestController(IEmailService emailService, IHolidayRequestRepository holidayRepository)
        {
            _emailService = emailService;
            _repository = holidayRepository;
        }

        [HttpPut]
        [Route("api/holidayrequest/{id}/approve")]
        public JsonResult Approve(long id)
        {
            HolidayRequest request = _repository.GetHolidayRequestById(id);
            request.Approve();
            _repository.Save();

            return Json(Ok());
        }

        [HttpDelete]
        [Route("api/holidayrequest/{id}")]
        public JsonResult Delete(long id)
        {
            HolidayRequest request = _repository.GetHolidayRequestById(id);

            if (request != null)
            {
                _repository.Delete(request);
                _repository.Save();

                return Json(Ok());
            }
            return Json(Ok());
        }
    }
}