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
using AutoMapper;
using MediatR;

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
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        private ApplicationContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,
            ITimesheetService tss, IEmailService emailService,
            ApplicationContext context, IMapper mapper, IMediator mediator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _timesheetService = tss;
            _emailService = emailService;
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
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
                        certs.Add(_mapper.Map<CertificationDto>(cert));
                    }
                    dtouser.Certifications = certs;
                }

                if (user.EmailNotifications.Count > 0)
                {
                    List<EmailNotificationDto> notifications = new List<EmailNotificationDto>();
                    foreach (EmailNotification not in user.EmailNotifications)
                    {
                        notifications.Add(_mapper.Map<EmailNotificationDto>(not));
                    }

                    dtouser.EmailNotifications = notifications;

                }
                if (user.HolidayRequests.Count > 0)
                {
                    List<HolidayRequestDto> holidays = new List<HolidayRequestDto>();
                    foreach (HolidayRequest hol in user.HolidayRequests)
                    {
                        holidays.Add(_mapper.Map<HolidayRequestDto>(hol));
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
            IEnumerable<HolidayRequest> holidayRequests = _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Where(b => b.UserId.ToString() == id)
                        .ToList();

            foreach (HolidayRequest hr in holidayRequests)
            {
                holidayRequestDtos.Add(_mapper.Map<HolidayRequestDto>(hr));
            }
            return holidayRequestDtos;
        }


        [HttpGet]
        [Route("api/supervisor/{id}/holidayrequests")]
        public IEnumerable<HolidayRequestDto> GetHolidayRequestsForApproval(string id)
        {
            List<HolidayRequestDto> holidayRequestDtos = new List<HolidayRequestDto>();
            IEnumerable<HolidayRequest> holidayRequests = _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Where(b => b.Approver.Id.ToString() == id)
                        .ToList();

            foreach (HolidayRequest hr in holidayRequests)
            {
                holidayRequestDtos.Add(_mapper.Map<HolidayRequestDto>(hr));
            }
            return holidayRequestDtos;
        }
        [HttpPut()]
        [Route("api/user/{id}/certificates")]
        public IActionResult Put(string id, [FromBody]CertificationDto t)
        {
            ApplicationUser user = GetUserIncludingCerts(id);
            Certification cert = user.AddCertification(t, _userManager).Result;

            if (cert != null)
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
            HolidayRequest hol = user.AddHolidayRequest(t, _userManager).Result;

            hol.Created(_mediator);

            if (hol != null)
            {
                return Ok(hol.Id);
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
            EmailNotification notification = user.AddNotification(t, _userManager, _mediator).Result;

            if (notification != null)
            {
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
                        certs.Add(_mapper.Map<CertificationDto>(cert));
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
                tsedto.Username = tse.Timesheet.Username;

                tsdto.TimesheetEntries.Add(tsedto);
            }

            return tsdto;
        }
    }
}