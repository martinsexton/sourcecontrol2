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

        long SaveClient(Client b);
        void AddProject(long clientId, Project proj);
        Client GetClientById(long id);
        IEnumerable<Client> GetClients();

        long SaveRate(LabourRate r);
        void UpdateRate(LabourRate r);
        void DeleteRate(long id);

        LabourRate GetRate(string role, DateTime date);
        IEnumerable<LabourRate> GetRates();
        LabourRate GetRateById(long id);

    }
}
