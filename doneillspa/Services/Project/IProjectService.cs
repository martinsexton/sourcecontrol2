using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface IProjectService
    {
        long InsertProject(Project b);
        void UpdateProject(Project b);
        void DeleteProject(Project b);
        IEnumerable<Project> GetProjects();
        Project GetProjectById(long id);

        long InsertClient(Client b);
        void UpdateClient(Client b);
        Client GetClientById(long id);
        IEnumerable<Client> GetClients();
    }
}
