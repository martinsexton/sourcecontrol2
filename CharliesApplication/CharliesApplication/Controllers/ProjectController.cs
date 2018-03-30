using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.DataAccess;
using CharliesApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace CharliesApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/Project")]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _repository;

        public ProjectController(IProjectRepository repository)
        {
            _repository = repository;

            if (!_repository.GetProjects().Any())
            {
                //Create Test Project
                Project p = new Project();
                p.Name = "Test Project";

                _repository.InsertProject(p);
                _repository.Save();

            }
        }

        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return _repository.GetProjects();
        }
    }
}