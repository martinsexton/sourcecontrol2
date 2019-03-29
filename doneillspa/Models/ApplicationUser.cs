using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public ICollection<Certification> Certifications { get; set; }
        public ICollection<HolidayRequest> HolidayRequests { get; set; }
        public ICollection<EmailNotification> EmailNotifications { get; set; }
    }
}
