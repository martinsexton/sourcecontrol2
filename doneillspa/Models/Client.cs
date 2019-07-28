using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Client
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project> Projects { get; set; }

        public void AddProject(Project proj)
        {
            Projects.Add(proj);
        }
    }
}
