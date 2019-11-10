using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using doneillspa.Services.Email;
using doneillspa.Dtos;
using doneillspa.Services;
using Microsoft.AspNetCore.SignalR;
using hub;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ITimesheetService _timesheetService;

        private readonly IEmailService _emailService;
        private readonly IHolidayService _holidayService;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,
            ITimesheetService tss, IEmailService emailService,
            IHolidayService hservice)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _timesheetService = tss;
            _emailService = emailService;
            _holidayService = hservice;
        }

        [HttpGet]
        [Route("api/user/roles")]
        public IEnumerable<IdentityRole<Guid>> GetRoles()
        {
            return _roleManager.Roles.AsEnumerable();
        }

        [HttpGet]
        [Route("api/user/role/{role}")]
        public IEnumerable<ApplicationUserDto> GetUsersWithRole(string role)
        {
            List<ApplicationUserDto> usersWithRole = new List<ApplicationUserDto>();

            List<ApplicationUser> users = _userManager.Users.ToList();
            foreach (ApplicationUser user in users)
            {
                Task<IList<string>> roles = _userManager.GetRolesAsync(user);
                foreach (String r in roles.Result)
                {
                    if (r.Equals(role))
                    {
                        ApplicationUserDto dtouser = new ApplicationUserDto();
                        dtouser.Id = user.Id;
                        dtouser.FirstName = user.FirstName;
                        dtouser.Surname = user.Surname;
                        dtouser.PhoneNumber = user.PhoneNumber;
                        dtouser.Email = user.Email;

                        usersWithRole.Add(dtouser);
                    }
                }
            }
            return usersWithRole;
        }

        [HttpPut]
        [Route("api/user")]
        public IActionResult Put([FromBody]ApplicationUserDto p)
        {
            ApplicationUser user = GetUserById(p.Id.ToString()).Result;
            user.IsEnabled = p.IsEnabled;

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            if (result.Result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/user")]
        public IEnumerable<ApplicationUserDto> Get()
        {
            List<ApplicationUser> users = _userManager.Users.Include(r => r.Certifications).Include(r => r.EmailNotifications).Include(r => r.HolidayRequests).ToList();

            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            foreach (ApplicationUser user in users)
            {
                Task<IList<string>> roles = _userManager.GetRolesAsync(user);

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                dtouser.IsEnabled = user.IsEnabled;

                if (roles.Result.Count > 0)
                {
                    dtouser.Role = roles.Result.First();
                }
                if (user.Certifications.Count > 0)
                {
                    List<CertificationDto> certs = new List<CertificationDto>();
                    foreach (Certification cert in user.Certifications)
                    {
                        CertificationDto dtocert = CertificationToDto(cert);
                        certs.Add(dtocert);
                    }
                    dtouser.Certifications = certs;
                }

                if (user.EmailNotifications.Count > 0)
                {
                    List<EmailNotificationDto> notifications = new List<EmailNotificationDto>();
                    foreach (EmailNotification not in user.EmailNotifications)
                    {
                        EmailNotificationDto notdto = NotificationToDto(not);
                        notifications.Add(notdto);
                    }

                    dtouser.EmailNotifications = notifications;

                }
                if (user.HolidayRequests.Count > 0)
                {
                    List<HolidayRequestDto> holidays = new List<HolidayRequestDto>();
                    foreach (HolidayRequest hol in user.HolidayRequests)
                    {
                        HolidayRequestDto holdto = HolidayToDto(hol);
                        holidays.Add(holdto);
                    }

                    dtouser.HolidayRequests = holidays;
                }
                dtousers.Add(dtouser);
            }
            return dtousers;
        }

        [HttpGet]
        [Route("api/contractor")]
        public IEnumerable<ApplicationUserDto> GetContractors()
        {
            List<ApplicationUser> users = _userManager.Users.Include(r => r.Certifications).Include(r => r.EmailNotifications).Include(r => r.HolidayRequests).ToList();

            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();
            List<string> contractorRoles = new List<string>();
            contractorRoles.Add("Loc1");
            contractorRoles.Add("Loc2");
            contractorRoles.Add("Loc3");

            foreach (ApplicationUser user in users)
            {
                IList<string> roles = _userManager.GetRolesAsync(user).Result;
                if(roles.Count > 0)
                {
                    if (contractorRoles.Contains(roles.First()))
                    {
                        ApplicationUserDto dtouser = new ApplicationUserDto();
                        dtouser.Id = user.Id;
                        dtouser.FirstName = user.FirstName;
                        dtouser.Surname = user.Surname;
                        dtouser.PhoneNumber = user.PhoneNumber;
                        dtouser.Email = user.Email;
                        dtouser.IsEnabled = user.IsEnabled;
                        dtouser.Role = roles.First();

                        dtousers.Add(dtouser);
                    }
                }
            }
            return dtousers;
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public JsonResult Get(string id)
        {
            Task<ApplicationUser> user = GetUserById(id);

            JsonResult result = new JsonResult(user.Result.UserName);
            return result;
        }

        [HttpGet]
        [Route("api/user/{id}/timesheets")]
        public IEnumerable<TimesheetDto> GetTimesheets(string id)
        {
            List<TimesheetDto> timesheetsDtos = new List<TimesheetDto>();

            Task<ApplicationUser> user = GetUserById(id);

            JsonResult result = new JsonResult(user.Result.UserName);

            IEnumerable<Timesheet> timesheets = _timesheetService.GetTimesheetsByUser(result.Value.ToString()).OrderByDescending(r => r.WeekStarting);

            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(TimesheetToDto(ts));
            }
            return timesheetsDtos;
        }


        [HttpGet]
        [Route("api/user/{id}/holidayrequests")]
        public IEnumerable<HolidayRequestDto> GetHolidayRequestsForUser(string id)
        {
            List<HolidayRequestDto> holidayRequestDtos = new List<HolidayRequestDto>();
            IEnumerable<HolidayRequest> holidayRequests = _holidayService.GetHolidayRequestsForUser(id);

            foreach (HolidayRequest hr in holidayRequests)
            {
                HolidayRequestDto dto = HolidayToDto(hr);
                holidayRequestDtos.Add(dto);
            }
            return holidayRequestDtos;
        }


        [HttpGet]
        [Route("api/supervisor/{id}/holidayrequests")]
        public IEnumerable<HolidayRequestDto> GetHolidayRequestsForApproval(string id)
        {
            List<HolidayRequestDto> holidayRequestDtos = new List<HolidayRequestDto>();
            IEnumerable<HolidayRequest> holidayRequests = _holidayService.GetHolidayRequestsForApprover(id);

            foreach (HolidayRequest hr in holidayRequests)
            {
                HolidayRequestDto dto = HolidayToDto(hr);
                holidayRequestDtos.Add(dto);
            }
            return holidayRequestDtos;
        }
        [HttpPut()]
        [Route("api/user/{id}/certificates")]
        public IActionResult Put(string id, [FromBody]CertificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            Certification cert = CertificationFromDto(user, t);
            user.Certifications.Add(cert);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            if (result.Result.Succeeded)
            {
                return Ok(cert.Id);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut()]
        [Route("api/user/{id}/holidayrequests")]
        public IActionResult Put(string id, [FromBody]HolidayRequestDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            HolidayRequest holiday = HolidayFromDto(user,t);
            user.HolidayRequests.Add(holiday);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            IdentityResult r = result.Result;
            if (result.Result.Succeeded)
            {
                holiday.Created(_emailService);
                return Ok(holiday.Id);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut()]
        [Route("api/user/{id}/reset")]
        public IActionResult Put(string id, [FromBody]PasswordReset d)
        {
            ApplicationUser user = _userManager.FindByIdAsync(d.UserId).Result;
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            IdentityResult result = _userManager.ResetPasswordAsync(user, token, d.Password).Result;
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut()]
        [Route("api/user/{id}/notifications")]
        public IActionResult Put(string id, [FromBody]EmailNotificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            EmailNotification notification = NotificationFromDto(user, t);

            //Default Activation Date to right now for the time being.
            notification.ActivationDate = DateTime.UtcNow;

            user.EmailNotifications.Add(notification);
            Task<IdentityResult> result = _userManager.UpdateAsync(user);

            if (result.Result.Succeeded)
            {
                notification.Created(_emailService);
                return Ok(notification.Id);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/user/name/{name}")]
        public ApplicationUserDto GetByName(string name)
        {
            List<ApplicationUserDto> dtousers = new List<ApplicationUserDto>();

            List<ApplicationUser> users = _userManager.Users.Where(r => r.UserName.Equals(name)).
                Include(r => r.Certifications).Include(r => r.HolidayRequests).ToList();

            foreach (ApplicationUser user in users)
            {
                Task<IList<string>> roles = _userManager.GetRolesAsync(user);

                ApplicationUserDto dtouser = new ApplicationUserDto();
                dtouser.Id = user.Id;
                dtouser.FirstName = user.FirstName;
                dtouser.Surname = user.Surname;
                dtouser.PhoneNumber = user.PhoneNumber;
                dtouser.Email = user.Email;
                if (roles.Result.Count > 0)
                {
                    dtouser.Role = roles.Result.First();
                }
                if (user.Certifications.Count > 0)
                {
                    List<CertificationDto> certs = new List<CertificationDto>();
                    foreach (Certification cert in user.Certifications)
                    {
                        CertificationDto dtocert = new CertificationDto();
                        dtocert.CreatedDate = cert.CreatedDate;
                        dtocert.Description = cert.Description;
                        dtocert.Expiry = cert.Expiry;
                        dtocert.Id = cert.Id;

                        certs.Add(dtocert);
                    }
                    dtouser.Certifications = certs;
                }
                dtousers.Add(dtouser);
            }
            return dtousers.First();
        }

        private ApplicationUser GetUserIncludingCerts(string id)
        {
            return _userManager.Users.Include(r => r.Certifications).Include(r => r.EmailNotifications).Include(r => r.HolidayRequests).Where(r => r.Id.ToString() == id).FirstOrDefault();
        }

        private async Task<ApplicationUser> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return user;
            }
            //User not found
            return await Task.FromResult<ApplicationUser>(null);
        }

        private async Task<ApplicationUser> GetUserByName(string name)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                return user;
            }
            //User not found
            return await Task.FromResult<ApplicationUser>(null);
        }

        private EmailNotification NotificationFromDto(ApplicationUser user, EmailNotificationDto dto)
        {
            EmailNotification notification = new EmailNotification();
            notification.DestinationEmail = user.Email;
            notification.Body = dto.Body;
            notification.Subject = dto.Subject;
            notification.User = user;
            notification.UserId = user.Id;
            notification.ActivationDate = new DateTime();

            return notification;
        }

        private EmailNotificationDto NotificationToDto(EmailNotification not)
        {
            EmailNotificationDto notdto = new EmailNotificationDto();
            notdto.Subject = not.Subject;
            notdto.Body = not.Body;
            notdto.DestinationEmail = not.DestinationEmail;
            notdto.Id = not.Id;
            notdto.ActivationDate = not.ActivationDate;

            return notdto;
        }


        private HolidayRequest HolidayFromDto(ApplicationUser user, HolidayRequestDto dto)
        {
            HolidayRequest holiday = new HolidayRequest();
            holiday.FromDate = dto.FromDate;
            holiday.Days = dto.Days;
            holiday.RequestedDate = DateTime.UtcNow;
            holiday.Approver = GetUserById(dto.ApproverId).Result;
            holiday.User = user;

            return holiday;
        }

        private HolidayRequestDto HolidayToDto(HolidayRequest hol)
        {
            HolidayRequestDto dto = new HolidayRequestDto();
            dto.Id = hol.Id;
            dto.FromDate = hol.FromDate;
            dto.Days = hol.Days;
            dto.ApproverId = hol.Approver.Id.ToString();
            dto.Status = hol.Status.ToString();
            dto.RequestedDate = hol.RequestedDate;

            return dto;
        }

        private Certification CertificationFromDto(ApplicationUser user, CertificationDto dto)
        {
            Certification cert = new Certification();
            cert.CreatedDate = dto.CreatedDate;
            cert.Description = dto.Description;
            cert.Expiry = dto.Expiry;
            cert.User = user;
            cert.UserId = user.Id;

            return cert;
        }

        private CertificationDto CertificationToDto(Certification cert)
        {
            CertificationDto dtocert = new CertificationDto();
            dtocert.CreatedDate = cert.CreatedDate;
            dtocert.Description = cert.Description;
            dtocert.Expiry = cert.Expiry;
            dtocert.Id = cert.Id;

            return dtocert;
        }

        private TimesheetDto TimesheetToDto(Timesheet ts)
        {
            TimesheetDto tsdto = new TimesheetDto();
            tsdto.DateCreated = ts.DateCreated;
            tsdto.Id = ts.Id;
            tsdto.Owner = ts.Owner;
            tsdto.Role = ts.Role;
            tsdto.Username = ts.Username;
            tsdto.WeekStarting = ts.WeekStarting;
            tsdto.Status = ts.Status.ToString();

            tsdto.TimesheetEntries = new List<TimesheetEntryDto>();
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                TimesheetEntryDto tsedto = new TimesheetEntryDto();
                tsedto.DateCreated = tse.DateCreated;
                tsedto.Day = tse.Day;
                tsedto.Details = tse.Details;
                tsedto.EndTime = tse.EndTime;
                tsedto.Id = tse.Id;
                tsedto.Code = tse.Code;
                tsedto.StartTime = tse.StartTime;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            return tsdto;
        }
    }
}