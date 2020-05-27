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
                tse.Chargeable = this.IsEntryChargeable(tse);
            }
        }

        public void Submitted(IMediator mediator)
        {
            Status = TimesheetStatus.Submitted;
            mediator.Publish(new TimesheetSubmitted { Username = this.Username });
        }

        public void Approved(IMediator mediator)
        {
            Status = TimesheetStatus.Approved;
            mediator.Publish(new TimesheetApproved { OwnerId = this.Owner.ToString(), WeekStarting = this.WeekStarting });
        }

        public void Rejected(IMediator mediator)
        {
            Status = TimesheetStatus.Rejected;
            mediator.Publish(new TimesheetRejected { OwnerId = this.Owner.ToString(), WeekStarting = this.WeekStarting });
        }

        public void AddTimesheetNote(IMediator mediator, TimesheetNote note)
        {
            //Set date created on timesheet entry
            note.DateCreated = DateTime.UtcNow;
            TimesheetNotes.Add(note);

            mediator.Publish(new TimesheetNoteCreated { OwnerId = this.Owner.ToString(), NoteDetails = note.Details });
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

        #endregion
    }
}
