using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Notifications
{
    public class TimesheetApproved : INotification
    {
        public string OwnerId { get; set; }
        public DateTime WeekStarting { get; set; }
    }
}
