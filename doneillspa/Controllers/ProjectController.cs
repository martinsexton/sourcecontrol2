﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    //[Route("api/project")]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _repository;

        public ProjectController(IProjectRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/project")]
        public IEnumerable<Project> Get()
        {
            return _repository.GetProjects();
        }

        [HttpGet]
        [Route("api/project/{id}")]
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

        [HttpDelete]
        [Route("api/project/{id}")]
        public JsonResult Delete(long id)
        {
            var existingProject = _repository.GetProjectById(id);

            if (existingProject != null)
            {
                _repository.DeleteProject(existingProject);
                _repository.Save();

                return Json(Ok());
            }

            return Json(Ok());
        }

        [HttpPost]
        [Route("api/project")]
        public IActionResult Post([FromBody]Project project)
        {
            if (project == null)
            {
                return BadRequest();
            }

            _repository.InsertProject(project);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _repository.Save();

            return Ok();
        }

        [HttpPut]
        [Route("api/project/{id}")]
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