using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountsController : Controller
    {
        private readonly ApplicationContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager, ApplicationContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        // POST api/accounts 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationDetails details)
        {

            var newUser = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                FirstName = details.FirstName,
                Surname = details.Surname,
                Email = details.Email,
                PhoneNumber = details.Phone,
                UserName = details.FirstName.ToUpper() + details.Surname.ToUpper()
            };

            //Password supplied must have non numeric, uppercase and digits in them in order to be saved
            var result = await _userManager.CreateAsync(newUser, details.Password);

            //If user created successfully then add role to user if provided
            if (result.Succeeded)
            {
                //Add Role to user
                if (!String.IsNullOrEmpty(details.Role))
                {
                    result = await _userManager.AddToRoleAsync(newUser, details.Role);
                }
            }

            if (!result.Succeeded) return new BadRequestObjectResult(result);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult("Account created");
        }

    }
}