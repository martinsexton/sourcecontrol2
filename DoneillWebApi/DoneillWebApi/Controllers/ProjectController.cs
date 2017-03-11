using DoneillWebApi.Models;
using persistancelayer;
using persistancelayer.api;
using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoneillWebApi.Controllers
{
    public class ProjectController : ApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Project> Get()
        {
            IPersistanceLayer pl = new PersistanceLayer();
            List<IProject> projects = pl.RetrieveProjects();
            List<Project> projectsForDisplay = new List<Project>();

            foreach(IProject p in projects){
                Project np = new Project();
                np.Name = p.getName();
                np.StartDate = p.getStartDate();
                np.ContactNumber = p.getContactNumber();
                np.Details = p.getDetails();

                projectsForDisplay.Add(np);
            }

            return projectsForDisplay;

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
            IPersistanceLayer pl = new PersistanceLayer();
            pl.CreateProject(value);
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
