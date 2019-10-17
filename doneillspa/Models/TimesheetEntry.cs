using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Email;

namespace doneillspa.Models
{
    public class TimesheetEntry
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public Timesheet Timesheet { get; set; }
    }
}
