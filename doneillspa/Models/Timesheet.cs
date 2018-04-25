using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Timesheet
    {
        public long Id { get; set; }
        public DateTime WeekStarting { get; set; }
        public Guid Owner { get; set; }
        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }
    }
}
