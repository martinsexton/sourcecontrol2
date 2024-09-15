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
        public bool IsActive { get; set; }
        public Tenant Tenant { get; set; }

        public ICollection<Project> Projects { get; set; }

        private Client(){}

        public Client(string name)
        {
            Name = name;
            IsActive = true;
        }

        public void AddProject(Project proj)
        {
            Projects.Add(proj);
        }

        public void Activate()
        {
            this.IsActive = true;
        }

        public void Disable()
        {
            this.IsActive = false;

            //If disabling a client, we need to disable projects also
            foreach(Project p in this.Projects)
            {
                p.Disable();
            }
        }
    }
}
