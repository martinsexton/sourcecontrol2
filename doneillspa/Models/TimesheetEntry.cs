using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class TimesheetEntry
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Project { get; set; }
    }
}
