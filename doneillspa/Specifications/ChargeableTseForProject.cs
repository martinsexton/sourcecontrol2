using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Specifications
{
    public class ChargeableTseForProject
    {
        private string projCode;

        public ChargeableTseForProject(string projectCode)
        {
            projCode = projectCode;
        }

        public bool IsSatisfied(TimesheetEntry tse)
        {
            return (tse.Chargeable && tse.Code.Equals(this.projCode));
        }
    }
}
