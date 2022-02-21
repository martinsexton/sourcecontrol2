using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class TimesheetEntryDto
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public string Username { get; set; }
        public bool Chargeable { get; set; }
    }
}
