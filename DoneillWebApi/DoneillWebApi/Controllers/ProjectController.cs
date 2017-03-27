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
                np.identifier = p.getIdentifier();
                np.Name = p.getName();
                np.StartDate = p.getStartDate();
                np.ContactNumber = p.getContactNumber();
                np.Details = p.getDetails();
                np.isActive = p.getIsActive();

                projectsForDisplay.Add(np);
            }

            return projectsForDisplay;

        }

        // GET api/values/5
        [HttpGet]
        public Project Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/values
        [HttpPost]
        public void Post(Project value)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            pl.CreateProject(value);
        }

        // PUT api/values/5
        [HttpPost]
        public void Post(int id, Project p)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            pl.UpdateProject(id, p);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
