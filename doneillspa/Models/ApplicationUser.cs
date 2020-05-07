using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using doneillspa.Services.Email;
using doneillspa.Dtos;

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

        public async Task<HolidayRequest> AddHolidayRequest(HolidayRequestDto request, UserManager<ApplicationUser> _userManager)
        {
            HolidayRequest holiday = HolidayFromDto(request, _userManager);
            HolidayRequests.Add(holiday);

            Task<IdentityResult> result = _userManager.UpdateAsync(this);

            if (result.Result.Succeeded)
            {
                return holiday;
            }
            else
            {
                return null;
            }
        }

        public async Task<Certification> AddCertification(CertificationDto cert, UserManager<ApplicationUser> _userManager)
        {
            Certification certification = CertificationFromDto(cert);
            Certifications.Add(certification);

            Task<IdentityResult> result = _userManager.UpdateAsync(this);

            if (result.Result.Succeeded)
            {
                return certification;
            }
            else
            {
                return null;
            }
        }

        public async Task<EmailNotification> AddNotification(EmailNotificationDto not, UserManager<ApplicationUser> _userManager, IEmailService _emailService)
        {
            EmailNotification notification = NotificationFromDto(not);

            //Default Activation Date to right now for the time being.
            notification.ActivationDate = DateTime.UtcNow;

            EmailNotifications.Add(notification);
            Task<IdentityResult> result = _userManager.UpdateAsync(this);

            if (result.Result.Succeeded)
            {
                notification.Created(_emailService);
                return notification;
            }
            else
            {
                return null;
            }
        }

        private HolidayRequest HolidayFromDto(HolidayRequestDto dto, UserManager<ApplicationUser> _userManager)
        {
            HolidayRequest holiday = new HolidayRequest();
            holiday.FromDate = dto.FromDate;
            holiday.Days = dto.Days;
            holiday.RequestedDate = DateTime.UtcNow;
            holiday.Approver = GetUserById(dto.ApproverId, _userManager).Result;
            holiday.User = this;

            return holiday;
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

        private Certification CertificationFromDto(CertificationDto dto)
        {
            Certification cert = new Certification();
            cert.CreatedDate = dto.CreatedDate;
            cert.Description = dto.Description;
            cert.Expiry = dto.Expiry;
            cert.User = this;
            cert.UserId = Id;

            return cert;
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
