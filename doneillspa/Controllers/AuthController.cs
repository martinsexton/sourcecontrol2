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

namespace doneillspa.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;


        public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;


            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }


        // POST api/auth/login 
        [HttpPost("login")]
        public async Task<JsonResult> Post([FromBody]RegistrationDetails credentials)
        {
            JsonResult result;
            var expiresIn = TimeSpan.FromMinutes(5);

            var identity = await GetClaimsIdentity(credentials.FirstName+credentials.Surname, credentials.Password);
            if (identity == null)
            {
                // Serialize and return the response 
                var errorResponse = new
                {
                    id = string.Empty,
                    auth_token = string.Empty,
                    expires_in = (int)expiresIn.TotalSeconds,
                    error = "Invalid username or password"
                };
                result = new JsonResult(errorResponse);
            }
            else
            {
                // Serialize and return the response 
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
                // get the user to verifty 
                ApplicationUser userToVerify = await _userManager.FindByNameAsync(userName.ToUpper());

                if (userToVerify != null)
                {
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