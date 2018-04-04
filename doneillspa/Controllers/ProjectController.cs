﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    [Route("api/project")]
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
                p.IsActive = true;
                p.Details = "UCC";

                _repository.InsertProject(p);
                _repository.Save();

            }
        }

        [HttpGet]
        public IEnumerable<Project> Get()
        {
            return _repository.GetProjects();
        }

        [HttpGet("{id}")]
        public JsonResult Get(long id)
        {
            var item = _repository.GetProjectById(id);

            if (item == null)
            {
                return Json(Ok());
            }
            JsonResult result = new JsonResult(item);
            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }

            _repository.InsertProject(project);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return CreatedAtRoute("Get", new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody]Project p)
        {
            if (p == null || p.Id != id)
            {
                return BadRequest();
            }

            var existingProject = _repository.GetProjectById(p.Id);

            if (existingProject == null)
            {
                return NotFound();
            }

            existingProject.Name = p.Name;
            existingProject.Details = p.Details;
            existingProject.IsActive = p.IsActive;

            _repository.UpdateProject(existingProject);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return new NoContentResult();
        }
    }
}