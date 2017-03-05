using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomiRepository.api
{
    public interface IRepository
    {
        void recordAuthenticationRequest(IAuthenticationRequest request);
    }
}
