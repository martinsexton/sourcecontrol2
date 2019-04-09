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

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ITimesheetRepository _repository;
        private readonly IEmailService _emailService;
        private readonly IHolidayRequestRepository _holidayRepository;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, ITimesheetRepository repository, IHolidayRequestRepository holidayRepository, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repository = repository;
            _emailService = emailService;
            _holidayRepository = holidayRepository;
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
                foreach(String r in roles.Result)
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

                if (user.EmailNotifications.Count > 0)
                {
                    List<EmailNotificationDto> notifications = new List<EmailNotificationDto>();
                    foreach (EmailNotification not in user.EmailNotifications)
                    {
                        EmailNotificationDto notdto = new EmailNotificationDto();
                        notdto.Subject = not.Subject;
                        notdto.Body = not.Body;
                        notdto.DestinationEmail = not.DestinationEmail;
                        notdto.Id = not.Id;

                        notifications.Add(notdto);
                    }

                    dtouser.EmailNotifications = notifications;

                }
                if (user.HolidayRequests.Count > 0)
                {
                    List<HolidayRequestDto> holidays = new List<HolidayRequestDto>();
                    foreach (HolidayRequest hol in user.HolidayRequests)
                    {
                        HolidayRequestDto holdto = new HolidayRequestDto();
                        holdto.FromDate = hol.FromDate;
                        holdto.Days = hol.Days;
                        holdto.Status = hol.Status.ToString();

                        holidays.Add(holdto);
                    }

                    dtouser.HolidayRequests = holidays;
                }
                dtousers.Add(dtouser);
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

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheetsByUser(result.Value.ToString()).OrderByDescending(r => r.WeekStarting);

            foreach (Timesheet ts in timesheets)
            {
                timesheetsDtos.Add(ConvertToDto(ts));
            }
            return timesheetsDtos;
        }


        [HttpGet]
        [Route("api/user/{id}/holidayrequests")]
        public IEnumerable<HolidayRequestDto> GetHolidayRequestsForUser(string id)
        {
            List<HolidayRequestDto> holidayRequestDtos = new List<HolidayRequestDto>();

            IEnumerable<HolidayRequest> holidayRequests = _holidayRepository.GetHolidayRequestsForUser(id);

            foreach (HolidayRequest hr in holidayRequests)
            {
                HolidayRequestDto dto = new HolidayRequestDto();
                dto.Id = hr.Id;
                dto.FromDate = hr.FromDate;
                dto.Days = hr.Days;
                dto.ApproverId = hr.Approver.Id.ToString();
                dto.Status = hr.Status.ToString();
                dto.RequestedDate = hr.RequestedDate;

                holidayRequestDtos.Add(dto);
            }
            return holidayRequestDtos;
        }


        [HttpGet]
        [Route("api/supervisor/{id}/holidayrequests")]
        public IEnumerable<HolidayRequestDto> GetHolidayRequestsForApproval(string id)
        {
            List<HolidayRequestDto> holidayRequestDtos = new List<HolidayRequestDto>();

            IEnumerable<HolidayRequest> holidayRequests = _holidayRepository.GetHolidayRequestsForApprover(id);

            foreach (HolidayRequest hr in holidayRequests)
            {
                HolidayRequestDto dto = new HolidayRequestDto();
                dto.Id = hr.Id;
                dto.FromDate = hr.FromDate;
                dto.Days = hr.Days;
                dto.ApproverId = hr.Approver.Id.ToString();
                dto.Status = hr.Status.ToString();
                dto.RequestedDate = hr.RequestedDate;

                holidayRequestDtos.Add(dto);
            }
            return holidayRequestDtos;
        }
        [HttpPut()]
        [Route("api/user/{id}/certificates")]
        public IActionResult Put(string id, [FromBody]CertificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            Certification cert = new Certification();
            cert.CreatedDate = t.CreatedDate;
            cert.Description = t.Description;
            cert.Expiry = t.Expiry;
            cert.User = user;
            cert.UserId = user.Id;

            user.Certifications.Add(cert);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            IdentityResult r = result.Result;

            return Ok(cert.Id);
        }

        [HttpPut()]
        [Route("api/user/{id}/holidayrequests")]
        public IActionResult Put(string id, [FromBody]HolidayRequestDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);
            ApplicationUser approver = GetUserById(t.ApproverId).Result;

            HolidayRequest holiday = new HolidayRequest();
            holiday.FromDate = t.FromDate;
            holiday.Days = t.Days;
            holiday.RequestedDate = DateTime.UtcNow;
            holiday.Status = HolidayRequestStatus.New;
            holiday.Approver = approver;

            user.HolidayRequests.Add(holiday);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);
            IdentityResult r = result.Result;

            _emailService.SendMail("doneill@hotmail.com", approver.Email, "Holiday Request", string.Format("{0} has requested holiday from {1} for {2} days.", user.FirstName, holiday.FromDate, holiday.Days), "", string.Empty, string.Empty);

            return Ok(holiday.Id);
        }

        [HttpPut()]
        [Route("api/user/{id}/notifications")]
        public IActionResult Put(string id, [FromBody]EmailNotificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);

            EmailNotification notification = new EmailNotification();
            notification.DestinationEmail = user.Email;
            notification.Body = t.Body;
            notification.Subject = t.Subject;
            notification.User = user;
            notification.UserId = user.Id;
            notification.ActivationDate = new DateTime();

            user.EmailNotifications.Add(notification);

            Task<IdentityResult> result = _userManager.UpdateAsync(user);

            notification.Send(_emailService);

            return Ok(notification.Id);
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

        private TimesheetDto ConvertToDto(Timesheet ts)
        {
            TimesheetDto tsdto = new TimesheetDto();
            tsdto.DateCreated = ts.DateCreated;
            tsdto.Id = ts.Id;
            tsdto.Owner = ts.Owner;
            tsdto.Role = ts.Role;
            tsdto.Username = ts.Username;
            tsdto.WeekStarting = ts.WeekStarting;

            tsdto.TimesheetEntries = new List<TimesheetEntryDto>();
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                TimesheetEntryDto tsedto = new TimesheetEntryDto();
                tsedto.DateCreated = tse.DateCreated;
                tsedto.Day = tse.Day;
                tsedto.Details = tse.Details;
                tsedto.EndTime = tse.EndTime;
                tsedto.Id = tse.Id;
                tsedto.Project = tse.Project;
                tsedto.StartTime = tse.StartTime;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            return tsdto;
        }
    }
}