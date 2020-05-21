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

        #endregion
    }
}
