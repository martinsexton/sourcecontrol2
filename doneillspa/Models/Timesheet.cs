using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Email;
using hub;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

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
        public ICollection<TimesheetNote> TimesheetNotes { get; set; }

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

        public void Updated(UserManager<ApplicationUser> userManager, IEmailService emailService, TimesheetDto ts, IHubContext<Chat> hub)
        {
            UpdateStatus(userManager, emailService, ts, hub);
        }

        private void UpdateStatus(UserManager<ApplicationUser> userManager, IEmailService emailService, TimesheetDto ts, IHubContext<Chat> hub)
        {
            if (ts.Status.Equals("Approved"))
            {
                Status = TimesheetStatus.Approved;
                SendMail(userManager, emailService, "Timesheet Approved", 
                    String.Format("Your timesheet for week beginning {0} has been approved", this.WeekStarting.ToShortDateString()));
            }
            else if (ts.Status.Equals("Rejected"))
            {
                Status = TimesheetStatus.Rejected;
                SendMail(userManager, emailService, "Timesheet Rejected",
                    String.Format("Your timesheet for week beginning {0} has been Rejected", this.WeekStarting.ToShortDateString()));
            }
            else if (ts.Status.Equals("Submitted"))
            {
                Status = TimesheetStatus.Submitted;
                //Send message to any client to inform that a timesheet has been approved.
                hub.Clients.All.SendAsync("timesheetsubmitted", "Timesheet Submitted For " + this.Username);
            }
        }

        public void AddTimesheetNote(UserManager<ApplicationUser> userManager, IEmailService emailService, TimesheetNote note)
        {
            //Set date created on timesheet entry
            note.DateCreated = DateTime.UtcNow;
            TimesheetNotes.Add(note);

            SendMail(userManager, emailService, "New Timesheet Note Created", note.Details);
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

            double rate = GetRate(Role, DateTime.UtcNow, Rates);

            Dictionary<string, double> hoursPerDay = RetrieveBreakdownOfHoursPerDay(proj);

            PopulateLabourDetail(Rates, detail, hoursPerDay);

            return detail;
        }

        public void PopulateLabourDetail(List<LabourRate> Rates, LabourWeekDetail detail, Dictionary<string,double> hoursPerDay)
        {
            double rate = GetRate(Role, DateTime.UtcNow, Rates);

            foreach (var item in hoursPerDay)
            {
                double minutesWorked = item.Value;
                switch (Role)
                {
                    case "Administrator":
                        detail.AdministratorCost += ((minutesWorked / 60) * 10);
                        break;
                    case "Supervisor":
                        detail.SupervisorCost += ((minutesWorked / 60) * rate);
                        break;
                    case "ChargeHand":
                        detail.ChargehandCost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR1":
                        detail.ElecR1Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR2":
                        detail.ElecR2Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR3":
                        detail.ElecR3Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Temp":
                        detail.TempCost += ((minutesWorked / 60) * rate);
                        break;
                    case "First Year Apprentice":
                        detail.FirstYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Second Year Apprentice":
                        detail.SecondYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Third Year Apprentice":
                        detail.ThirdYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Fourth Year Apprentice":
                        detail.FourthYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    default:
                        break;
                }
            }
        }

        private double GetRate(string role, DateTime onDate, List<LabourRate> Rates)
        {
            foreach (LabourRate r in Rates)
            {
                if (r.Role.Equals(role) && r.EffectiveFrom <= onDate && (r.EffectiveTo == null || r.EffectiveTo >= onDate))
                {
                    return r.RatePerHour;
                }
            }
            //Default Value
            return 0.0;
        }

        public Dictionary<string,double> RetrieveBreakdownOfHoursPerDay(string proj)
        {
            Dictionary<string, double> hoursPerDay = new Dictionary<string, double>();
            foreach (TimesheetEntry tse in this.TimesheetEntries)
            {
                if (tse.Code.Equals(proj))
                {
                    TimeSpan startTimespan = TimeSpan.Parse(tse.StartTime);
                    TimeSpan endTimespan = TimeSpan.Parse(tse.EndTime);
                    TimeSpan result = endTimespan - startTimespan;

                    if (!hoursPerDay.ContainsKey(tse.Day))
                    {
                        hoursPerDay.Add(tse.Day, result.TotalMinutes);
                    }
                    else
                    {
                        double totalMins = hoursPerDay[tse.Day];
                        totalMins += result.TotalMinutes;
                        hoursPerDay[tse.Day] = totalMins;
                    }
                }
            }
            //Update each of the entries to remove 30 mins for days where engineer worked >= 5 hours
            foreach (string key in hoursPerDay.Keys.ToList())
            {
                double minutesWorked = hoursPerDay[key];
                if (minutesWorked >= (5 * 60))
                {
                    minutesWorked = minutesWorked - 30;
                    hoursPerDay[key] = minutesWorked;
                }
            }
            return hoursPerDay;
        }

        private void SendMail(UserManager<ApplicationUser> userManager, IEmailService _emailService, string subject, string message)
        {
            ApplicationUser user = userManager.FindByIdAsync(this.Owner.ToString()).Result;
            //Contractors will not have an email, so check against sending email for these users.
            if (!String.IsNullOrEmpty(user.Email))
            {
                _emailService.SendMail("doneill@hotmail.com", user.Email, subject, message, "", string.Empty, string.Empty);
            }
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
