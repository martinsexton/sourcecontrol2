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
    public class NonChargeableTimeController : Controller
    {
        private readonly INonChargeableTimeRepository _repository;

        public NonChargeableTimeController(INonChargeableTimeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/nonchargeabletime")]
        public IEnumerable<NonChargeableTime> Get()
        {
            return _repository.GetNonChargeableTime();
        }
    }
}