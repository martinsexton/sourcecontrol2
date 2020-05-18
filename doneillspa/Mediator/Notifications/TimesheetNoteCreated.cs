using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Notifications
{
    public class TimesheetNoteCreated : INotification
    {
        public string UserEmail { get; set; }
        public string NoteDetails { get; set; }
    }
}
