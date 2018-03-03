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
                _repository.InsertBaby(new Baby { FirstName = "Charlie", Surname = "Sexton", Sex = "Male", BirthDetails = new BirthDetails { Weight = 5, BirthDate = DateTime.Now, Hospital = "Rotunda" } });
                _repository.InsertBaby(new Baby { FirstName = "Holly", Surname = "Sexton", Sex = "Female" });
                //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
                _repository.Save();
            }
        }

        // GET: api/Baby
        [HttpGet]
        public IEnumerable<Baby> Get()
        {
            return _repository.GetBabies();
        }

        // GET: api/Baby/5
        //[HttpGet("{id}", Name = "Get")]
        //public IActionResult Get(long id)
        //{
        //    var item = _repository.GetBabyById(id);

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }
        //    return new ObjectResult(item);
        //}
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(long id)
        {
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