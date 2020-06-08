using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Specifications
{
    public class ApprovedTimesheetForProject
    {
        private string projCode;

        public ApprovedTimesheetForProject(string projectCode)
        {
            projCode = projectCode;
        }

        public bool IsSatisfied(Timesheet ts)
        {
            bool satisfied = false;

            if (ts.IsApproved())
            {
                foreach(TimesheetEntry tse in ts.TimesheetEntries)
                {
                    if (tse.Code.Equals(projCode))
                    {
                        satisfied = true;
                    }
                }
            }

            return satisfied;
        }
    }
}
