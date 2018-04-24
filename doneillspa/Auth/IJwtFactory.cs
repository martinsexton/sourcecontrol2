using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace doneillspa.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, TimeSpan expires); 
        ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id, IList<string> roles);

    }
}
