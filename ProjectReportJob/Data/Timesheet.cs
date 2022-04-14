using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectReportJob.Data
{
    public class Timesheet
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime WeekStarting { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public Guid Owner { get; set; }
        public TimesheetStatus Status { get; set; }


        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }

    }
}
