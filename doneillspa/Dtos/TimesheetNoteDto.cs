using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class TimesheetNoteDto
    {
        public long Id { get; set; }
        public string Details { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
