using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Models
{
    public class Timesheet
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public long Id { get; set; }
        public string Username { get; set; }
        public DateTime WeekStarting { get; set; }
        public Guid Owner { get; set; }

        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }
    }
}
