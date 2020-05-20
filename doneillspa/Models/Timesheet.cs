using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using doneillspa.Mediator.Notifications;
using doneillspa.Services.Email;
using hub;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace doneillspa.Models
{
    public class Timesheet
    {
        #region Properties

        public long Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime WeekStarting { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid Owner { get; set; }
        public TimesheetStatus Status { get; set; }

        #endregion

        #region Collections

        public ICollection<TimesheetEntry> TimesheetEntries { get; set; }
        public ICollection<TimesheetNote> TimesheetNotes { get; set; }

        #endregion

        #region Actions
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

        public void Submitted(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            Status = TimesheetStatus.Submitted;
            mediator.Publish(new TimesheetSubmitted { Username = this.Username });
        }

        public void Approved(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            Status = TimesheetStatus.Approved;
            ApplicationUser user = userManager.FindByIdAsync(this.Owner.ToString()).Result;
            mediator.Publish(new TimesheetApproved { UserEmail = user.Email, WeekStarting = this.WeekStarting });
        }

        public void Rejected(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            Status = TimesheetStatus.Rejected;
            ApplicationUser user = userManager.FindByIdAsync(this.Owner.ToString()).Result;
            mediator.Publish(new TimesheetRejected { UserEmail = user.Email, WeekStarting = this.WeekStarting });
        }

        public void AddTimesheetNote(UserManager<ApplicationUser> userManager, IMediator mediator, TimesheetNote note)
        {
            ApplicationUser user = userManager.FindByIdAsync(this.Owner.ToString()).Result;

            //Set date created on timesheet entry
            note.DateCreated = DateTime.UtcNow;
            TimesheetNotes.Add(note);

            mediator.Publish(new TimesheetNoteCreated { UserEmail = user.Email, NoteDetails = note.Details });
        }

        public void AddTimesheetEntry(TimesheetEntry entry)
        {
            //Set date created on timesheet entry
            entry.DateCreated = DateTime.UtcNow;
            //Determine if timesheet entry is chargeable or not.
            entry.Chargeable = IsEntryChargeable(entry);

            TimesheetEntries.Add(entry);
        }

        #endregion

        #region Helper Methods

        public bool IsEntryChargeable(TimesheetEntry entry)
        {
            //Always true right now, but we can add logic here later to control if its chargeable or not.
            return true;
        }

        public LabourWeekDetail BuildLabourWeekDetails(List<LabourRate> Rates, string proj)
        {
            //Retrieve details from timesheet and populate the LabourWeekDetail object
            LabourWeekDetail detail = new LabourWeekDetail(proj, this.WeekStarting);
            Dictionary<string, double> hoursPerDay = RetrieveBreakdownOfHoursPerDay(proj);

            PopulateLabourDetail(Rates, detail, hoursPerDay);

            return detail;
        }

        private void PopulateLabourDetail(List<LabourRate> Rates, LabourWeekDetail detail, Dictionary<string, double> hoursPerDay)
        {
            double rate = GetRate(Role, this.WeekStarting.Date, Rates);

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
                    case "Loc1":
                        detail.Loc1Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Loc2":
                        detail.Loc2Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Loc3":
                        detail.Loc3Cost += ((minutesWorked / 60) * rate);
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

        public Dictionary<string, double> RetrieveBreakdownOfHoursPerDay(string proj)
        {
            Dictionary<string, double> hoursPerDay = new Dictionary<string, double>();
            foreach (TimesheetEntry tse in this.TimesheetEntries)
            {
                if (!tse.Chargeable)
                {
                    continue;
                }
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
            RemoveLunchBreak(hoursPerDay);

            return hoursPerDay;
        }

        private void RemoveLunchBreak(Dictionary<string, double> hoursPerDay)
        {
            //Update each of the entries to remove 30 mins for days where engineer worked >= 5 hours
            foreach (string key in hoursPerDay.Keys.ToList())
            {
                double minutesWorked = hoursPerDay[key];
                if (((key.Equals("Sat") || key.Equals("Sun")) && minutesWorked > (5 * 60))
                    || (!(key.Equals("Sat") || key.Equals("Sun")) && minutesWorked >= (5 * 60)))
                {
                    minutesWorked = minutesWorked - 30;
                    hoursPerDay[key] = minutesWorked;
                }
            }
        }

        #endregion
    }
}
