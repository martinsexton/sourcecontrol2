using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using doneillspa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _service;
        public ProjectController(IProjectService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/project")]
        public IEnumerable<ProjectDto> Get()
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _service.GetProjects();
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
            var item = _service.GetProjectById(id);

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
            var existingProject = _service.GetProjectById(id);

            if (existingProject != null)
            {
                _service.DeleteProject(existingProject);

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

           long id = _service.SaveProject(project);

            return Ok(id);
        }

        [HttpPut]
        [Route("api/project")]
        public IActionResult Put([FromBody]Project p)
        {
            _service.UpdateProject(p);
            return new NoContentResult();
        }
    }
}