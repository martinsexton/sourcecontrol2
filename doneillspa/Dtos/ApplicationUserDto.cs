using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Dtos;

namespace doneillspa.Models
{
    public class ApplicationUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsEnabled { get; set; }
        public ICollection<CertificationDto> Certifications { get; set; }
        public ICollection<EmailNotificationDto> EmailNotifications { get; set; }
        public ICollection<HolidayRequestDto> HolidayRequests { get; set; }
    }
}
