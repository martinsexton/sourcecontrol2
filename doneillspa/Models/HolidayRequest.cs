using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Dtos;
using doneillspa.Models.State;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

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

        public HolidayRequest()
        {
            Status = HolidayRequestStatus.New;
        }

        public void Created(IEmailService emailService)
        {
            emailService.SendMail("doneill@hotmail.com", Approver.Email, "Holiday Request", string.Format("{0} has requested holiday from {1} for {2} days.", User.FirstName, FromDate, Days), "", string.Empty, string.Empty);
        }

        //Method triggerd by controller when HolidayRequest has been updated.
        public void Updated(HolidayRequestDto dto, ICalendarService _calendarService, IEmailService _emailService, ITimesheetService tss)
        {
            if (dto.Status.Equals(HolidayRequestStatus.Approved.ToString()))
            {
                GetState().Approve(_calendarService, _emailService, tss);
            }
            else if (dto.Status.Equals(HolidayRequestStatus.Rejected.ToString()))
            {
                GetState().Reject(_emailService);
            }
        }

        public void TransitionTo(HolidayRequestStatus state)
        {
            Status = state;
        }
        private IHolidayRequestState GetState()
        {
            switch (Status)
            {
                case HolidayRequestStatus.New:
                    return new HolidayRequestNewState(this);
                default:
                    return new HolidayRequestApproveState(this);
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
