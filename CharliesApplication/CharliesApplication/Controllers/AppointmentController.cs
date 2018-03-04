using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CharliesApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/Appointment")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public JsonResult Get(long id)
        {
            //Instead of returning all objects in graph, we could return HATEOS links 
            //for all collections on a Baby object.
            var item = _repository.GetAppointmentById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }
    }
}