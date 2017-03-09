using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using DoneillServer.Models;
using System.Web.Http.Cors;

namespace DoneillServer.Controllers
{
    public class ProjectsController : ApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            Project p1 = new Project();
            p1.Name = "P1 name";
            p1.FromDate = DateTime.Now;
            p1.ContactNumber = "234234";
            p1.Details = "sddf";


            Project p2 = new Project();
            p2.Name = "P2 name";
            p2.FromDate = DateTime.Now;
            p2.ContactNumber = "234234";
            p2.Details = "sddf";

            return new Project[] { p1, p2 };
        }

        // GET api/values/5
        [HttpGet]
        public Project Get(int id)
        {
            //return "value";
            Project p = new Project();
            p.Name = "Test";

            return p;
        }

        // POST api/values
        [HttpPost]
        public void Post(Project value)
        {
            String test = value.Details;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
