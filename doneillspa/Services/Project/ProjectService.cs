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

        public long SaveProject(Project b)
        {
            return _projectRepository.InsertProject(b);
        }

        public void UpdateProject(Project proj)
        {
            if (proj != null)
            {
                Project existingProject = GetProjectById(proj.Id);
                if (existingProject != null)
                {
                    existingProject.Name = proj.Name;
                    existingProject.Details = proj.Details;
                    existingProject.IsActive = proj.IsActive;
                    existingProject.Code = proj.Code;

                    _projectRepository.UpdateProject(existingProject);
                }
            }
        }

        public void DeleteProject(Project b)
        {
            _projectRepository.DeleteProject(b);
        }

        public Project GetProjectById(long id)
        {
            return _projectRepository.GetProjectById(id);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepository.GetProjects();
        }

        public long SaveClient(Client b)
        {
            return _clientRepository.InsertClient(b);
        }

        public void AddProject(long clientId, Project proj)
        {
            Client c = GetClientById(clientId);
            c.AddProject(proj);
            _clientRepository.UpdateClient(c);
        }

        public Client GetClientById(long id)
        {
            return _clientRepository.GetClientById(id);
        }

        public IEnumerable<Client> GetClients()
        {
            return _clientRepository.GetClients();
        }

        public long SaveRate(LabourRate r)
        {
            return _rateRepository.InsertRate(r);
        }

        public void UpdateRate(LabourRate r)
        {
            LabourRate existingRate = GetRateById(r.Id);

            if(existingRate != null)
            {
                existingRate.EffectiveFrom = r.EffectiveFrom;
                existingRate.EffectiveTo = r.EffectiveTo;
                existingRate.RatePerHour = r.RatePerHour;
                existingRate.OverTimeRatePerHour = r.OverTimeRatePerHour;
            }

            _rateRepository.UpdateRate(existingRate);
        }

        public void DeleteRate(LabourRate r)
        {
            _rateRepository.DeleteRate(r);
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
    }
}
