using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface IProjectRepository
    {
        void InsertProject(Project b);
        void UpdateProject(Project b);
        IEnumerable<Project> GetProjects();
        Project GetProjectById(long id);
        void Save();
    }
}
