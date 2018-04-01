using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _context;

        public ProjectRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Project GetProjectById(long id)
        {
            return _context.Project
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }

        public IEnumerable<Project> GetProjects()
        {
            return _context.Project.ToList();
        }

        public void InsertProject(Project p)
        {
            _context.Project.Add(p);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateProject(Project p)
        {
            _context.Entry(p).State = EntityState.Modified;
        }
    }
}
