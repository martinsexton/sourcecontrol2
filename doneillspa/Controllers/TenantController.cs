using AutoMapper;
using doneillspa.DataAccess;
using doneillspa.Dtos;
using doneillspa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace doneillspa.Controllers
{
    //Secure this Web API so that only a token provided with the roles satisfing the Policy called employee will have access 
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class TenantController : Controller
    {
        ApplicationContext _context;

        public TenantController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/tenant")]
        public IEnumerable<TenantDto> Get()
        {
            List<TenantDto> dtos = new List<TenantDto>();

            IEnumerable<Tenant> tenants = _context.Tenant;

            foreach (Tenant t in tenants)
            {
                TenantDto dto = new TenantDto();
                dto.Id = t.Id;
                dto.Name = t.Name;

                dtos.Add(dto);
            }
            return dtos;
        }
    }
}
