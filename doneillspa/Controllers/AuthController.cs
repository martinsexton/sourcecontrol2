using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using doneillspa.Auth;
using doneillspa.Helpers;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private const string INVALID_LOGIN_ATTEMPT = "Invalid username or password";

        public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        [HttpPost("login")]
        public async Task<JsonResult> Post([FromBody]RegistrationDetails credentials)
        {
            JsonResult result;
            var expiresIn = TimeSpan.FromDays(7);
            //var expiresIn = TimeSpan.FromSeconds(10);

            var identity = await GetClaimsIdentity(credentials.FirstName+credentials.Surname, credentials.Password);
            if (identity == null)
            {
                var errorResponse = new
                {
                    id = string.Empty,
                    auth_token = string.Empty,
                    expires_in = (int)expiresIn.TotalSeconds,
                    error = INVALID_LOGIN_ATTEMPT
                };
                result = new JsonResult(errorResponse);
            }
            else
            {
                var response = new
                {
                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    auth_token = await _jwtFactory.GenerateEncodedToken(credentials.FirstName + credentials.Surname, identity, expiresIn),
                    expires_in = (int)expiresIn.TotalSeconds,
                    role = identity.Claims.Single(c => c.Type == "rol").Value
                };

                result = new JsonResult(response);
            }

            return result;
        }


        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                ApplicationUser userToVerify = await _userManager.FindByNameAsync(userName.ToUpper());

                if (userToVerify != null)
                {
                    HttpContext.Session.SetString("UserEmail", userToVerify.Email);

                    IList<string> roles = await _userManager.GetRolesAsync(userToVerify);

                    // check the credentials   
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, roles));
                    }
                }
            }
            // Credentials are invalid, or account doesn't exist 
            return await Task.FromResult<ClaimsIdentity>(null);
        }

    }
}