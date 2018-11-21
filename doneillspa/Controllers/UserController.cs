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

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("api/user/roles")]
        public IEnumerable<IdentityRole<Guid>> GetRoles()
        {
            return _roleManager.Roles.AsEnumerable();
        }

        [HttpGet]
        [Route("api/user")]
        public IEnumerable<ApplicationUserDto> Get()
        {
            List<ApplicationUser> users = _userManager.Users.Include(r => r.Certifications).ToList();

            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            foreach (ApplicationUser user in users)
            {
                Task<IList<string>> roles = _userManager.GetRolesAsync(user);

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                if (roles.Result.Count > 0)
                {
                    dtouser.Role = roles.Result.First();
                }
                if (user.Certifications.Count > 0)
                {
                    List<CertificationDto> certs = new List<CertificationDto>();
                    foreach (Certification cert in user.Certifications)
                    {
                        CertificationDto dtocert = new CertificationDto();
                        dtocert.CreatedDate = cert.CreatedDate;
                        dtocert.Description = cert.Description;
                        dtocert.Expiry = cert.Expiry;
                        dtocert.Id = cert.Id;

                        certs.Add(dtocert);
                    }
                    dtouser.Certifications = certs;
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

        [HttpGet]
        [Route("api/user/name/{name}")]
        public ApplicationUserDto GetByName(string name)
        {
            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            List<ApplicationUser> users = _userManager.Users.Where(r => r.UserName.Equals(name)).
                Include(r => r.Certifications).ToList();

            foreach (ApplicationUser user in users)
            {
                Task<IList<string>> roles = _userManager.GetRolesAsync(user);

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                if (roles.Result.Count > 0)
                {
                    dtouser.Role = roles.Result.First();
                }
                if (user.Certifications.Count > 0)
                {
                    List<CertificationDto> certs = new List<CertificationDto>();
                    foreach (Certification cert in user.Certifications)
                    {
                        CertificationDto dtocert = new CertificationDto();
                        dtocert.CreatedDate = cert.CreatedDate;
                        dtocert.Description = cert.Description;
                        dtocert.Expiry = cert.Expiry;
                        dtocert.Id = cert.Id;

                        certs.Add(dtocert);
                    }
                    dtouser.Certifications = certs;
                }
                dtousers.Add(dtouser);
            }
            return dtousers.First();

            ////Need to bring back the full user object not just the name below.
            //Task<ApplicationUser> user = GetUserByName(name);

            //JsonResult result = new JsonResult(user.Result.UserName);
            //return result;
        }

        [HttpGet]
        [Route("api/user/roles/{id}")]
        public IEnumerable<string> GetRoles(string id)
        {
            Task<ApplicationUser> user = GetUserById(id);
            Task<IList<string>> roles = _userManager.GetRolesAsync(user.Result);

            return roles.Result;
        }

        [HttpPut()]
        [Route("api/user/{id}")]
        public IActionResult Put(string id, [FromBody]CertificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            Certification cert = new Certification();
            cert.CreatedDate = t.CreatedDate;
            cert.Description = t.Description;
            cert.Expiry = t.Expiry;
            cert.User = user;
            cert.UserId = user.Id;

            user.Certifications.Add(cert);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            IdentityResult r = result.Result;

            return Ok(cert.Id);
        }

        private ApplicationUser GetUserIncludingCerts(string id)
        {
            return _userManager.Users.Include(r => r.Certifications).Where(r => r.Id.ToString() == id).FirstOrDefault();
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

        private async Task<ApplicationUser> GetUserByName(string name)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                return user;
            }
            //User not found
            return await Task.FromResult<ApplicationUser>(null);
        }
    }
}