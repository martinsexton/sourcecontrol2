using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CharliesApplication.Models;
using CharliesApplication.DataAccess;

namespace CharliesApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/Baby")]
    public class BabyController : Controller
    {
        private readonly IBabyRepository _repository;

        public BabyController(IBabyRepository repository)
        {
            _repository = repository;

            if (!_repository.GetBabies().Any())
            {
                Baby b1 = new Baby();
                b1.FirstName = "Charlie";
                b1.Surname = "Sexton";
                b1.Sex = "Male";
                b1.BirthDetails = new BirthDetails { Weight = 5, BirthDate = DateTime.Now, Hospital = "Rotunda" };
                b1.Appointments.Add(new Appointment { DueDate = DateTime.Now, Description = "Doctors appointment", Type = AppointmentType.Doctor });
                b1.Appointments.Add(new Appointment { DueDate = DateTime.Now.AddDays(7), Description = "Doctors appointment", Type = AppointmentType.Doctor });
                b1.Activities.Add(new Activity { TimeStamp = DateTime.Now, Description = "Got him to do some Tummy Time", Type = ActivityType.TummyTime });

                _repository.InsertBaby(b1);
                _repository.InsertBaby(new Baby { FirstName = "Holly", Surname = "Sexton", Sex = "Female" });
                //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
                _repository.Save();
            }
        }

        // GET: api/Baby
        [HttpGet]
        public IEnumerable<Baby> Get()
        {
            //TODO return this as HATEOS
            return _repository.GetBabies();
        }

        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(long id)
        {
            //Instead of returning all objects in graph, we could return HATEOS links 
            //for all collections on a Baby object.
            var item = _repository.GetBabyById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }

        // POST: api/Baby
        [HttpPost]
        public IActionResult Post([FromBody]Baby baby)
        {
            if (baby == null)
            {
                return BadRequest();
            }

            _repository.InsertBaby(baby);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return CreatedAtRoute("Get", new { id = baby.Id }, baby);
        }

        // PUT: api/Baby/5
        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Baby b)
        {
            if (b == null || b.Id != id)
            {
                return BadRequest();
            }

            var existingBaby = _repository.GetBabyById(b.Id);

            if (existingBaby == null)
            {
                return NotFound();
            }

            existingBaby.FirstName = b.FirstName;
            existingBaby.Surname = b.Surname;

            _repository.UpdateBaby(existingBaby);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}