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

        public void Updated(HolidayRequestDto dto, ICalendarService _calendarService, IEmailService _emailService, ITimesheetService tss)
        {
            if (dto.IsApproved())
            {
                GetState(_calendarService, _emailService, tss).Approve();
            }
            else if (dto.IsRejected())
            {
                GetState(_calendarService, _emailService, tss).Reject();
            }
        }

        public void TransitionTo(HolidayRequestStatus state)
        {
            Status = state;
        }

        private IHolidayRequestState GetState(ICalendarService calendarService, IEmailService emailService, ITimesheetService timesheetService)
        {
            switch (Status)
            {
                case HolidayRequestStatus.New:
                    return new HolidayRequestNewState(this, calendarService, emailService, timesheetService);
                default:
                    return new HolidayRequestApproveState(this, calendarService, emailService, timesheetService);
            }
        }
    }

}
