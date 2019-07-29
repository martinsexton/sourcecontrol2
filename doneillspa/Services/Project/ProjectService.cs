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
        private readonly IRateRepository _rateRepository;

        public ProjectService(IProjectRepository pr, IClientRepository cr, IRateRepository rr)
        {
            _projectRepository = pr;
            _clientRepository = cr;
            _rateRepository = rr;
        }
        public void DeleteProject(Project b)
        {
            _projectRepository.DeleteProject(b);
        }

        public void DeleteRate(LabourRate r)
        {
            _rateRepository.DeleteRate(r);
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

        public LabourRate GetRate(string role, DateTime date)
        {
            return _rateRepository.GetRate(role, date);
        }

        public LabourRate GetRateById(long id)
        {
            return _rateRepository.GetRateById(id);
        }

        public IEnumerable<LabourRate> GetRates()
        {
            return _rateRepository.GetRates();
        }

        public long InsertClient(Client b)
        {
            return _clientRepository.InsertClient(b);
        }

        public long InsertProject(Project b)
        {
            return _projectRepository.InsertProject(b);
        }

        public long InsertRate(LabourRate r)
        {
            return _rateRepository.InsertRate(r);
        }

        public void UpdateClient(Client b)
        {
            _clientRepository.UpdateClient(b);
        }

        public void UpdateProject(Project b)
        {
            _projectRepository.UpdateProject(b);
        }

        public void UpdateRate(LabourRate r)
        {
            _rateRepository.UpdateRate(r);
        }
    }
}
