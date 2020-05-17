using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Dtos;
using doneillspa.Mediator.Notifications;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;
using MediatR;

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

        public void Created(IMediator mediator)
        {
            mediator.Publish(new HolidayRequestCreated { ApproverEmail = this.Approver.Email, UserName = this.User.FirstName, FromDate = this.FromDate, Days = this.Days });
        }

        public void Updated(HolidayRequestDto dto, IMediator mediator)
        {
            if (dto.IsApproved())
            {
                mediator.Publish(new HolidayRequestApproved { UserName = this.User.FirstName + this.User.Surname, UserEmail = this.User.Email, FromDate = this.FromDate, Days = this.Days });
                Status = HolidayRequestStatus.Approved;
            }
            else if (dto.IsRejected())
            {
                mediator.Publish(new HolidayRequestRejected { UserName = this.User.FirstName + this.User.Surname, UserEmail = this.User.Email, FromDate = this.FromDate, Days = this.Days });
                Status = HolidayRequestStatus.Rejected;
            }
        }
    }

}
