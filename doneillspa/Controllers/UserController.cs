using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using doneillspa.Services.Email;
using doneillspa.Dtos;
using doneillspa.Services;
using AutoMapper;
using MediatR;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        private ApplicationContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,
            ApplicationContext context, IMapper mapper, IMediator mediator, ITimesheetRepository repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _timeSheetRepository = repository;
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPut]
        [Route("api/user")]
        public IActionResult Put([FromBody]ApplicationUserDto p)
        {
            ApplicationUser user = GetUserById(p.Id.ToString()).Result;
            user.IsEnabled = p.IsEnabled;
            user.Email = p.Email;
            user.PhoneNumber = p.PhoneNumber;

            //Add or update claim with new tenant id
            UpdateClaimsWithTenantDetails(user, p);


            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            if (result.Result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/user/{inactiveUsers}/{page}/{pageSize}")]
        public IEnumerable<ApplicationUserDto> Get([FromQuery] string tenant, bool inactiveUsers, int page = 1, int pageSize = 10)
        {
            List<ApplicationUser> users = _userManager.Users
                .Where(r => r.IsEnabled == !inactiveUsers)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            foreach (ApplicationUser user in users)
            {
                IList<string> roles = _userManager.GetRolesAsync(user).Result;
                IList<Claim> claims = _userManager.GetClaimsAsync(user).Result;

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                dtouser.IsEnabled = user.IsEnabled;
                
                if (roles.Count > 0)
                {
                    dtouser.Role = roles.First();
                }

                if(claims.Count > 0)
                {
                    foreach (Claim c in claims)
                    {
                        if (c.Type == "Tenant")
                        {
                            dtouser.TenantId = long.Parse(c.Value);
                        }
                    }
                }

                dtousers.Add(dtouser);
            }
            return dtousers;
        }

        [HttpGet]
        [Route("api/user/{inactiveUsers}/{filter}/{page}/{pageSize}")]
        public IEnumerable<ApplicationUserDto> GetBaseOnFilter(bool inactiveUsers, string filter, int page = 1, int pageSize = 10)
        {
            List<ApplicationUser> users = _userManager.Users
                .Where(r => r.UserName.Contains(filter) && r.IsEnabled == !inactiveUsers)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            foreach (ApplicationUser user in users)
            {
                IList<string> roles = _userManager.GetRolesAsync(user).Result;
                IList<Claim> claims = _userManager.GetClaimsAsync(user).Result;

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                dtouser.IsEnabled = user.IsEnabled;

                if (roles.Count > 0)
                {
                    dtouser.Role = roles.First();
                }

                if (claims.Count > 0)
                {
                    foreach (Claim c in claims)
                    {
                        if (c.Type == "Tenant")
                        {
                            dtouser.TenantId = long.Parse(c.Value);
                        }
                    }
                }

                dtousers.Add(dtouser);
            }
            return dtousers;
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public JsonResult Get(string id)
        {
            Task<ApplicationUser> user = GetUserById(id);

            JsonResult result = new JsonResult(user.Result.UserName);
            return result;
        }

        [HttpPut()]
        [Route("api/user/reset")]
        public IActionResult Put([FromBody]PasswordReset d)
        {
            ApplicationUser user = _userManager.FindByIdAsync(d.UserId).Result;
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            IdentityResult result = _userManager.ResetPasswordAsync(user, token, d.Password).Result;
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private void UpdateClaimsWithTenantDetails(ApplicationUser user, ApplicationUserDto dto)
        {
            if (dto.TenantId == 0) { return; }

            IList<Claim> claims = _userManager.GetClaimsAsync(user).Result;
            if (claims.Count > 0)
            {
                foreach (Claim c in claims)
                {
                    if (c.Type == "Tenant")
                    {
                        if (!c.Value.Equals(dto.TenantId.ToString()))
                        {
                            _userManager.RemoveClaimAsync(user, c).Wait();
                            _userManager.AddClaimAsync(user, new Claim("Tenant", dto.TenantId.ToString())).Wait();
                        }
                    }
                }
            }
            else
            {
                _userManager.AddClaimAsync(user, new Claim("Tenant", dto.TenantId.ToString())).Wait();
            }
        }

        private async Task<ApplicationUser> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return user;
            }
            //User not found
            return await Task.FromResult<ApplicationUser>(null);
        }
    }
}