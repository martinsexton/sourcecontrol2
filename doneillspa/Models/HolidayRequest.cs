using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Dtos;
using doneillspa.Services.Calendar;

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

        private void Approve(ICalendarService _calendarService)
        {
            Status = HolidayRequestStatus.Approved;
            //Create an event in Google Calendar.
            _calendarService.CreateEvent(FromDate, Days, User.FirstName + " " + User.Surname);
        }

        //Method triggerd by controller when HolidayRequest has been updated.
        public void Updated(HolidayRequestDto dto, ICalendarService _calendarService)
        {
            if (dto.Status.Equals(HolidayRequestStatus.Approved.ToString())
                && Status == HolidayRequestStatus.New)
            {
                Approve(_calendarService);
            }
        }
    }

    public enum HolidayRequestStatus
    {
        New = 1,
        Approved = 2,
        Rejected = 3
    }
}
