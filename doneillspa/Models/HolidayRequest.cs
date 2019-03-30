using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class HolidayRequest
    {
        public long Id { get; set; }

        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser Approver { get; set; }

        public DateTime RequestedDate { get; set; }
        public DateTime FromDate { get; set; }
        public int Days { get; set; }

        public HolidayRequestStatus Status { get; set; }
    }

    public enum HolidayRequestStatus
    {
        New = 1,
        Approved = 2,
        Rejected = 3
    }
}
