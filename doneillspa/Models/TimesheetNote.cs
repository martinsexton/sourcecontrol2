using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class TimesheetNote
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Details { get; set; }
        public Timesheet Timesheet { get; set; }
    }
}
