using doneillspa.Mediator.Notifications;
using hub;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Handlers
{
    public class HandleTimesheetSubmitted : INotificationHandler<TimesheetSubmitted>
    {
        IHubContext<Chat> _hub;

        public HandleTimesheetSubmitted(IHubContext<Chat> hub)
        {
            _hub = hub;
        }

        public Task Handle(TimesheetSubmitted notification, CancellationToken cancellationToken)
        {
            _hub.Clients.All.SendAsync("timesheetsubmitted", "Timesheet Submitted For " + notification.Username);
            return Task.CompletedTask;
        }
    }
}
