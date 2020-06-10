using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using doneillspa.Factories;
using doneillspa.Helpers;
using doneillspa.Mediator.Notifications;
using doneillspa.Services.Email;
using doneillspa.Specifications;
using hub;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

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
                tse.Chargeable = tse.IsEntryChargeable();
            }
        }

        public void Submitted(IMediator mediator)
        {
            Status = TimesheetStatus.Submitted;

            //Create a domain event for any side effects to register
            mediator.Publish(new TimesheetSubmitted { Username = this.Username });
        }

        public void Approved(IMediator mediator)
        {
            Status = TimesheetStatus.Approved;

            //Create a domain event for any side effects to register
            mediator.Publish(new TimesheetApproved { OwnerId = this.Owner.ToString(), WeekStarting = this.WeekStarting });
        }

        public void Rejected(IMediator mediator)
        {
            Status = TimesheetStatus.Rejected;

            //Create a domain event for any side effects to register
            mediator.Publish(new TimesheetRejected { OwnerId = this.Owner.ToString(), WeekStarting = this.WeekStarting });
        }

        public void AddTimesheetNote(IMediator mediator, TimesheetNote note)
        {
            //Set date created on timesheet entry
            note.DateCreated = DateTime.UtcNow;
            TimesheetNotes.Add(note);

            //Create a domain event for any side effects to register
            mediator.Publish(new TimesheetNoteCreated { OwnerId = this.Owner.ToString(), NoteDetails = note.Details });
        }

        public decimal CalculateHoursWorkedForProject(Timesheet ts, string code)
        {
            decimal hours = 0;
            ApprovedTseForProject approvedForProjectSpecification = new ApprovedTseForProject(code);

            foreach (TimesheetEntry tse in TimesheetEntries)
            {
                //If timesheet entry satisfies the specification then proceed
                if (!approvedForProjectSpecification.IsSatisfied(tse))
                    continue;

                hours += tse.DurationInHours();
            }
            return hours;
        }

        public void RecordAnnualLeaveForDay(DayOfWeek day)
        {
            TimesheetEntry tse = TimesheetFactory.CreateFullDayEntryForDay(Constants.Strings.Timesheets.NonChargeableCodes.AnnualLeave, day);
            AddTimesheetEntry(tse);
        }

        public void AddTimesheetEntry(TimesheetEntry entry)
        {
            //Set date created on timesheet entry
            entry.DateCreated = DateTime.UtcNow;
            //Determine if timesheet entry is chargeable or not.
            entry.Chargeable = entry.IsEntryChargeable();

            TimesheetEntries.Add(entry);
        }


        #endregion

        #region Helper Methods

        public bool IsApproved()
        {
            return Status == TimesheetStatus.Approved;
        }

        #endregion
    }
}
