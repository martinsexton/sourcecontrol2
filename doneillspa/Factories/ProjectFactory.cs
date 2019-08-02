using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Factories
{
    public static class ProjectFactory
    {
        public static Project CreateProject(string code, string name, string details, DateTime startDate)
        {
            Project p = new Project();
            p.Code = code;
            p.Name = name;
            p.Details = details;
            p.StartDate = startDate;
            p.IsActive = true;

            return p;
        }
    }
}
