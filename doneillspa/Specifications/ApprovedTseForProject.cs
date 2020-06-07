using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Specifications
{
    public class ApprovedTseForProject
    {
        private string projCode;

        public ApprovedTseForProject(string projectCode)
        {
            projCode = projectCode;
        }

        public bool IsSatisfied(TimesheetEntry tse)
        {
            return (tse.Timesheet.IsApproved() && tse.Code.Equals(this.projCode));
        }
    }
}
