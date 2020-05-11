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

        private Client(){}

        public Client(string name)
        {
            Name = name;
        }

        public void AddProject(Project proj)
        {
            Projects.Add(proj);
        }
    }
}
