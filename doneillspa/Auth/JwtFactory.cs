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
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IConfiguration _configuration;


         public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions, IConfiguration configuration)
         { 
             _jwtOptions = jwtOptions.Value;
            _configuration = configuration;
             ThrowIfInvalidOptions(_jwtOptions); 
         } 

 
         public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
         {
            var requestAt = DateTime.Now;
            var expiresIn = requestAt.Add(TimeSpan.FromMinutes(30));

            var claims = new[]
          {
                   new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                    identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
                };
            //Temo Add Claims
            identity.AddClaims(claims);

            var _keyByteArray = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var _signingKey = new SymmetricSecurityKey(_keyByteArray);

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

            // Create the JWT security token and encode it. 
            //var jwt = new JwtSecurityToken(
            //     issuer: _jwtOptions.Issuer,
            //     audience: _jwtOptions.Audience,
            //     claims: claims,
            //     notBefore: _jwtOptions.NotBefore,
            //     expires: _jwtOptions.Expiration,
            //     signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));


            var encodedJwt = handler.WriteToken(securityToken);



             return encodedJwt; 
         } 
 
 
         public ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id)
         { 
             return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[] 
             { 
                 new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id.ToString()), 
                 new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
             }); 
         } 
 
 
         /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns> 
         private static long ToUnixEpochDate(DateTime date)
           => (long) Math.Round((date.ToUniversalTime() - 
                                new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)) 
                               .TotalSeconds); 
 
 
         private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
         { 
             if (options == null) throw new ArgumentNullException(nameof(options)); 
 
 
             if (options.ValidFor <= TimeSpan.Zero) 
             { 
                 throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor)); 
             } 
 
 
             if (options.SigningCredentials == null) 
             { 
                 throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials)); 
             } 
 
 
             if (options.JtiGenerator == null) 
             { 
                 throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator)); 
             } 
         } 

    }
}
