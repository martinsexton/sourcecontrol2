using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public IActionResult Post([FromBody]RegistrationDetails details)
        {
            var userIdentity = new ApplicationUser();
            userIdentity.UserName = details.Email;
            userIdentity.PasswordHash = details.Password;

            //Save user here 

            return Ok();
        }
    }
}