using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Dtos;

namespace doneillspa.Models
{
    public class TimesheetDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime WeekStarting { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid Owner { get; set; }
        public string Status { get; set; }

        public ICollection<TimesheetEntryDto> TimesheetEntries { get; set; }
        public ICollection<TimesheetNoteDto> TimesheetNotes { get; set; }
    }
}
