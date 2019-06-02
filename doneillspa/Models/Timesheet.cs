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

        public void OnCreation()
        {
            //Setup new timesheets with a default value of New.
            Status = TimesheetStatus.New;

            DateTime todaysDate = DateTime.UtcNow;

            //Set the date created on timesheet
            DateCreated = todaysDate;

            foreach (TimesheetEntry tse in TimesheetEntries)
            {
                tse.DateCreated = todaysDate;
            }
        }

        public void Updated(TimesheetDto ts)
        {
            UpdateStatus(ts);
        }

        private void UpdateStatus(TimesheetDto ts)
        {
            if (ts.Status.Equals("Approved"))
            {
                Status = TimesheetStatus.Approved;
            }
            else if (ts.Status.Equals("Rejected"))
            {
                Status = TimesheetStatus.Rejected;
            }
            else if (ts.Status.Equals("Submitted"))
            {
                Status = TimesheetStatus.Submitted;
            }
        }

        public void AddTimesheetEntry(TimesheetEntry entry)
        {
            //Set date created on timesheet entry
            entry.DateCreated = DateTime.UtcNow;
            TimesheetEntries.Add(entry);
        }

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
