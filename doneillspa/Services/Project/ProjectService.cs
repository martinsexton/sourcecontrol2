using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;

        public ProjectService(IProjectRepository pr, IClientRepository cr)
        {
            _projectRepository = pr;
            _clientRepository = cr;
        }
        public void DeleteProject(Project b)
        {
            _projectRepository.DeleteProject(b);
        }

        public Client GetClientById(long id)
        {
            return _clientRepository.GetClientById(id);
        }

        public IEnumerable<Client> GetClients()
        {
            return _clientRepository.GetClients();
        }

        public Project GetProjectById(long id)
        {
            return _projectRepository.GetProjectById(id);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepository.GetProjects();
        }

        public long InsertClient(Client b)
        {
            return _clientRepository.InsertClient(b);
        }

        public long InsertProject(Project b)
        {
            return _projectRepository.InsertProject(b);
        }

        public void UpdateClient(Client b)
        {
            _clientRepository.UpdateClient(b);
        }

        public void UpdateProject(Project b)
        {
            _projectRepository.UpdateProject(b);
        }
    }
}
