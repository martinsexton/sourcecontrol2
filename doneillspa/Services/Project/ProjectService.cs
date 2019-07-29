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

        public ProjectService(IProjectRepository pr)
        {
            _projectRepository = pr;
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

        public long InsertProject(Project b)
        {
            return _projectRepository.InsertProject(b);
        }

        public void UpdateProject(Project b)
        {
            _projectRepository.UpdateProject(b);
        }
    }
}
