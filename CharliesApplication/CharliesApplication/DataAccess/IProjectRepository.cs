using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;

namespace CharliesApplication.DataAccess
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
