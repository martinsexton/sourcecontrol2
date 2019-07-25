using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Dtos;
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
        public IEnumerable<ProjectDto> Get()
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _repository.GetProjects();
            foreach(Project p in projects)
            {
                ProjectDto dto = new ProjectDto();
                dto.Client = p.OwningClient.Name;
                dto.Id = p.Id;
                dto.IsActive = p.IsActive;
                dto.Name = p.Name;
                dto.Code = p.Code;
                dto.StartDate = p.StartDate;
                dto.Details = p.Details;

                projectDtos.Add(dto);
            }
            return projectDtos;
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

           long id = _repository.InsertProject(project);

            return Ok(id);
        }

        [HttpPut]
        [Route("api/project")]
        public IActionResult Put([FromBody]Project p)
        {
            if (p == null)
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
            existingProject.Code = p.Code;

            _repository.UpdateProject(existingProject);

            return new NoContentResult();
        }
    }
}