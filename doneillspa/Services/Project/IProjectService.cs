using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface IProjectService
    {
        void UpdateProject(Project b);
        void DeleteProject(long id);
        IEnumerable<Project> GetProjects();
        Project GetProjectById(long id);

        long SaveClient(string b);
        long AddProject(long clientId, string code, string name, string details, DateTime startDate);
        Client GetClientById(long id);
        IEnumerable<Client> GetClients();

        long SaveRate(DateTime effectiveFrom, DateTime? effectiveTo, double ratePerHour, double overtimeRatePerHour, string role);
        void UpdateRate(LabourRate r);
        void DeleteRate(long id);

        LabourRate GetRate(string role, DateTime date);
        IEnumerable<LabourRate> GetRates();
        LabourRate GetRateById(long id);

    }
}
