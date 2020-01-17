using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Enumerations
    {
        public enum TimesheetStatus
        {
            New = 0,
            Submitted = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
