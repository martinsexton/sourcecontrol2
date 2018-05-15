using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/user")]
        public IEnumerable<ApplicationUser> Get()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            return users;
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
        [Route("api/user/{id}")]
        public IActionResult Put(string id, [FromBody]CertificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            Certification cert = new Certification();
            cert.CreatedDate = t.CreatedDate;
            cert.Description = t.Description;
            cert.CreatedDate = t.CreatedDate;
            cert.User = user;
            cert.UserId = user.Id;

            user.Certifications.Add(cert);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            IdentityResult r = result.Result;

            return new NoContentResult();
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
    }
}