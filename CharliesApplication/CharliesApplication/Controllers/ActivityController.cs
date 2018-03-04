using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CharliesApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/Activity")]
    public class ActivityController : Controller
    {
        private readonly IActivityRepository _repository;

        public ActivityController(IActivityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public JsonResult Get(long id)
        {
            //Instead of returning all objects in graph, we could return HATEOS links 
            //for all collections on a Baby object.
            var item = _repository.GetActivityById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }
    }
}