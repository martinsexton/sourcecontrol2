using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Mediator.Notifications
{
    public class HolidayRequestApproved : INotification
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public int Days { get; set; }
    }
}
