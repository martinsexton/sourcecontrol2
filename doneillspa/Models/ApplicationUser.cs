using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using doneillspa.Services.Email;
using doneillspa.Dtos;
using MediatR;

namespace doneillspa.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public bool? IsEnabled { get; set; }
        public ICollection<Certification> Certifications { get; set; }
        public ICollection<HolidayRequest> HolidayRequests { get; set; }
        public ICollection<EmailNotification> EmailNotifications { get; set; }

        public async Task<EmailNotification> AddNotification(EmailNotificationDto not, UserManager<ApplicationUser> _userManager, IMediator _mediator)
        {
            EmailNotification notification = NotificationFromDto(not);

            //Default Activation Date to right now for the time being.
            notification.ActivationDate = DateTime.UtcNow;

            EmailNotifications.Add(notification);
            Task<IdentityResult> result = _userManager.UpdateAsync(this);

            if (result.Result.Succeeded)
            {
                notification.Created(_mediator);
                return notification;
            }
            else
            {
                return null;
            }
        }

        private async Task<ApplicationUser> GetUserById(string id, UserManager<ApplicationUser> _userManager)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return user;
            }
            //User not found
            return await Task.FromResult<ApplicationUser>(null);
        }

        private EmailNotification NotificationFromDto(EmailNotificationDto dto)
        {
            EmailNotification notification = new EmailNotification();
            notification.DestinationEmail = Email;
            notification.Body = dto.Body;
            notification.Subject = dto.Subject;
            notification.User = this;
            notification.UserId = this.Id;
            notification.ActivationDate = new DateTime();

            return notification;
        }
    }
}
