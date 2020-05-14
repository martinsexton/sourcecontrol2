using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class HolidayRequestDto
    {
        public long Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime RequestedDate { get; set; }
        public int Days { get; set; }
        public string ApproverId { get; set; }
        public string Status { get; set; }

        public bool IsApproved()
        {
            return Status.Equals(HolidayRequestStatus.Approved.ToString());
        }

        public bool IsRejected()
        {
            return Status.Equals(HolidayRequestStatus.Rejected.ToString());
        }
    }
}
