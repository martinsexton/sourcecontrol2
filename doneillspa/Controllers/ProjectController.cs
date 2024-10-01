using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProjectController(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("api/project")]
        public IEnumerable<ProjectDto> Get()
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _context.Project.Include(b => b.OwningClient).OrderBy(b => b.Name).ToList();
            foreach (Project p in projects)
            {
                projectDtos.Add(_mapper.Map<ProjectDto>(p));
            }
            return projectDtos;
        }


        [HttpGet]
        [Route("api/project/client/{clientId}/{inactiveProjects}/{page}/{pageSize}")]
        public IEnumerable<ProjectDto> Get(int clientId, bool inactiveProjects, int page = 1, int pageSize = 10)
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _context.Project
                .Where(b => b.OwningClient.Id == clientId && b.IsActive != inactiveProjects)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(b => b.OwningClient).OrderByDescending(b => b.Id).ToList();
            foreach (Project p in projects)
            {
                projectDtos.Add(_mapper.Map<ProjectDto>(p));
            }
            return projectDtos;
        }

        [HttpGet]
        [Route("api/activeproject")]
        public IEnumerable<ProjectDto> GetActiveProjects()
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _context.Project.Include(b => b.OwningClient).Where(b => b.IsActive).OrderBy(b => b.Name).ToList();
            foreach (Project p in projects)
            {
                projectDtos.Add(_mapper.Map<ProjectDto>(p));
            }
            return projectDtos;
        }

        [HttpGet]
        [Route("api/activeproject/{code}")]
        public IEnumerable<ProjectDto> GetActiveProjectsByCode(string code)
        {
            List<ProjectDto> projectDtos = new List<ProjectDto>();

            IEnumerable<Project> projects = _context.Project.Include(b => b.OwningClient).Where(b => b.IsActive && b.Code.Contains(code)).OrderBy(b => b.Name).ToList();
            foreach (Project p in projects)
            {
                projectDtos.Add(_mapper.Map<ProjectDto>(p));
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

            if(proj.IsActive != p.IsActive)
            {
                if (p.IsActive)
                {
                    //Call activate on project domain
                    proj.Activate();
                }
                else
                {
                    //call disable on project domain
                    proj.Disable();
                }
            }
            else
            {
                //Update fields
                proj.Name = p.Name;
                proj.Details = p.Details;
                //proj.IsActive = p.IsActive;
                proj.Code = p.Code;
            }


            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}