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
using Microsoft.EntityFrameworkCore;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class ProjectController : Controller
    {
        ApplicationContext _context;

        public ProjectController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/project")]
        public IEnumerable<ProjectDto> Get()
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _context.Project.Include(b => b.OwningClient).ToList();
            foreach (Project p in projects)
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

        [HttpDelete]
        [Route("api/project/{id}")]
        public JsonResult Delete(long id)
        {
            Project proj = _context.Project
                        .Include(b => b.OwningClient)
                        .Where(b => b.Id == id)
                        .FirstOrDefault();

            _context.Entry(proj).State = EntityState.Deleted;
            _context.SaveChanges();

            return Json(Ok());
        }

        [HttpPut]
        [Route("api/project")]
        public IActionResult Put([FromBody]ProjectDto p)
        {
            Project proj = _context.Project
                        .Include(b => b.OwningClient)
                        .Where(b => b.Id == p.Id)
                        .FirstOrDefault();

            //Update fields
            proj.Name = p.Name;
            proj.Details = p.Details;
            proj.IsActive = p.IsActive;
            proj.Code = p.Code;

            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}