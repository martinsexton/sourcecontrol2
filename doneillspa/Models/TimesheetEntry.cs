using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class TimesheetEntry
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Project { get; set; }
        public string Details { get; set; }
    }
}
