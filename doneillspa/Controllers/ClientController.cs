﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Factories;
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
    public class ClientController : Controller
    {
        ApplicationContext _context;

        public ClientController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("api/client")]
        public IActionResult Post([FromBody]ClientDto client)
        {
            if (client == null) return BadRequest();

            Client c = new Client(client.Name);
            _context.Client.Add(c);

            _context.SaveChanges();

            return Ok(c.Id);
        }

        [HttpGet]
        [Route("api/client")]
        public IEnumerable<ClientDto> Get()
        {
            List<ClientDto> dtos = new List<ClientDto>();

            IEnumerable<Client> clients = _context.Client.Include(b => b.Projects).ToList();
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
                    pdto.Details = proj.Details;
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
        public IActionResult Put(long id, [FromBody]ProjectDto p)
        {
            Client client = _context.Client.Include(b => b.Projects).Where(b => b.Id == id).FirstOrDefault();

            Project proj = new Project(p.Code, p.Name, p.Details, p.StartDate);
            client.AddProject(proj);

            _context.SaveChanges();

            return Ok(proj.Id);
        }
    }
}