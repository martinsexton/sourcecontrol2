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
    }
}
