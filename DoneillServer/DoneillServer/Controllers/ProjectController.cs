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
            p1.identifier = 1;
            p1.Name = "Honan Chaple Upgrade";
            p1.FromDate = DateTime.Now;
            p1.ContactNumber = "087 7485948";
            p1.Details = "Need to upgrade heating system";


            Project p2 = new Project();
            p2.identifier = 2;
            p2.Name = "Burkes Garage";
            p2.FromDate = DateTime.Now;
            p2.ContactNumber = "021 485748";
            p2.Details = "Switches needed in staff room";

            Project p3 = new Project();
            p3.identifier = 1;
            p3.Name = "UCC";
            p3.FromDate = DateTime.Now;
            p3.ContactNumber = "021 948394";
            p3.Details = "Alarm system to upgrade";


            Project p4 = new Project();
            p4.identifier = 2;
            p4.Name = "Riordans Pub";
            p4.FromDate = DateTime.Now;
            p4.ContactNumber = "087 84758937";
            p4.Details = "Work needed in kitchen";

            return new Project[] { p1, p2,p3,p4 };
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
