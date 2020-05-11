using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Factories;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IRateRepository _rateRepository;

        public ProjectService(IProjectRepository pr, IRateRepository rr)
        {
            _projectRepository = pr;
            _rateRepository = rr;
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

        public void DeleteProject(long id)
        {
            Project proj = GetProjectById(id);
            _projectRepository.DeleteProject(proj);
        }

        public Project GetProjectById(long id)
        {
            return _projectRepository.GetProjectById(id);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _projectRepository.GetProjects();
        }

        public long SaveRate(DateTime effectiveFrom, DateTime? effectiveTo, double ratePerHour, double overtimeRatePerHour, string role)
        {
            LabourRate rate = LabourRateFactory.CreateLabourRate(effectiveFrom, effectiveTo, ratePerHour, overtimeRatePerHour, role);

            return _rateRepository.InsertRate(rate);
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

        public void DeleteRate(long id)
        {
            LabourRate rate = GetRateById(id);
            _rateRepository.DeleteRate(rate);
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
