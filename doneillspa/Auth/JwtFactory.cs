using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace doneillspa.Auth
{
    public class JwtFactory : IJwtFactory
    {
        //private readonly JwtIssuerOptions _jwtOptions;
        private readonly IConfiguration _configuration;


         public JwtFactory(IConfiguration configuration)
         { 
            _configuration = configuration; 
         } 

 
         public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, TimeSpan expires)
         {
            var expiresIn = DateTime.Now.Add(expires);

            var _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {

                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                Subject = identity,
                Expires = expiresIn,
                NotBefore = DateTime.Now.Subtract(TimeSpan.FromMinutes(30))
            });

            return handler.WriteToken(securityToken);
         } 
 
 
         public ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id, IList<string> roles)
         {
            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[] 
             { 
                 new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id.ToString()), 
             });

            //Add roles for the user
            foreach(string role in roles)
            {
                identity.AddClaim(new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, role));
            }

            return identity;
         } 
 
 
         /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns> 
         private static long ToUnixEpochDate(DateTime date)
           => (long) Math.Round((date.ToUniversalTime() - 
                                new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)) 
                               .TotalSeconds); 

    }
}
