using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface IClientRepository
    {

        long InsertClient(Client b);
        void UpdateClient(Client b);
        Client GetClientById(long id);
        IEnumerable<Client> GetClients();
    }
}
