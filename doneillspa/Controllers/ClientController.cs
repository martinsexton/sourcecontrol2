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
    public class ClientController : Controller
    {
        private readonly IProjectService _projectService;

        public ClientController(IProjectService ps)
        {
            _projectService = ps;
        }

        [HttpPost]
        [Route("api/client")]
        public IActionResult Post([FromBody]Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            long id = _projectService.SaveClient(client);

            return Ok(id);
        }

        [HttpGet]
        [Route("api/client")]
        public IEnumerable<ClientDto> Get()
        {
            List<ClientDto> dtos = new List<ClientDto>();

            IEnumerable<Client> clients = _projectService.GetClients();
            foreach (Client c in clients)
            {
                List<ProjectDto> projects = new List<ProjectDto>();

                ClientDto dto = new ClientDto();
                dto.Id = c.Id;
                dto.Name = c.Name;

                foreach(Project proj in c.Projects)
                {
                    ProjectDto pdto = new ProjectDto();
                    pdto.Client = c.Name;
                    pdto.Id = proj.Id;
                    pdto.IsActive = proj.IsActive;
                    pdto.StartDate = proj.StartDate;
                    pdto.Name = proj.Name;
                    pdto.Code = proj.Code;

                    projects.Add(pdto);
                }

                dto.Projects = projects;

                dtos.Add(dto);
            }
            return dtos;
        }

        [HttpPut()]
        [Route("api/client/{id}/projects")]
        public IActionResult Put(long id, [FromBody]Project p)
        {
            _projectService.AddProject(id, p);
            return Ok(p.Id);
        }
    }
}