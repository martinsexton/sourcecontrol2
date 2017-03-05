using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignOnRepository.api
{
    public interface ISignOnRepository
    {
        List<ISignOnRequest> retrieveAuthenticationRequests();
        ISignOnRequest retrieveAuthenticationRequestById(int id);
    }
}
