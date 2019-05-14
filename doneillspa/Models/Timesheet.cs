using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Models
{
    public class Timesheet
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime WeekStarting { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid Owner { get; set; }
        public TimesheetStatus Status { get; set; }

        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }

        public LabourWeekDetail BuildLabourWeekDetails(List<LabourRate> Rates, string proj)
        {
            //Retrieve details from timesheet and populate the LabourWeekDetail object
            LabourWeekDetail detail = new LabourWeekDetail();

            detail.Week = this.WeekStarting;

            foreach (TimesheetEntry tse in this.TimesheetEntries)
            {
                if (!String.IsNullOrEmpty(proj))
                {
                    //Only populate details for relevant project
                    if (tse.Project.Equals(proj))
                    {
                        tse.PopulateLabourDetail(detail, Rates);
                    }
                }
                else
                {
                    tse.PopulateLabourDetail(detail, Rates);
                }
            }

            return detail;
        }
    }

    public enum TimesheetStatus
    {
        New = 1,
        Submitted = 2,
        Approved = 3,
        Rejected = 4
    }
}
