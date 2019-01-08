using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class LabourDetailsController : Controller
    {
        private readonly ITimesheetRepository _repository;

        public LabourDetailsController(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/labourdetails")]
        public IEnumerable<LabourWeekDetail> Get()
        {
            Dictionary<DateTime, LabourWeekDetail> details = new Dictionary<DateTime, LabourWeekDetail>();

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach(Timesheet ts in timesheets)
            {
                if (!details.ContainsKey(ts.WeekStarting.Date))
                {
                    LabourWeekDetail detail = new LabourWeekDetail();
                    detail.Week = ts.WeekStarting;

                    UpdateLaboutWeekDurations(detail, ts);
                    details.Add(ts.WeekStarting.Date, detail);
                }
                else
                {
                    LabourWeekDetail detail = details[ts.WeekStarting.Date];
                    UpdateLaboutWeekDurations(detail, ts);
                }
            }

            return details.Values.AsEnumerable<LabourWeekDetail>();
        }

        private void UpdateLaboutWeekDurations(LabourWeekDetail detail, Timesheet ts)
        {
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                TimeSpan startTimespan = TimeSpan.Parse(tse.StartTime);
                TimeSpan endTimespan = TimeSpan.Parse(tse.EndTime);
                TimeSpan result = endTimespan - startTimespan;

                switch (ts.Role)
                {
                    case "Administrator":
                        detail.AdministratorMinutes += result.TotalMinutes;
                        break;
                    case "Supervisor":
                        detail.SupervisorMinutes += result.TotalMinutes;
                        break;
                    case "ChargeHand":
                        detail.ChargehandMinutes += result.TotalMinutes;
                        break;
                    case "ElectR1":
                        detail.ElecR1Minutes += result.TotalMinutes;
                        break;
                    case "ElectR2":
                        detail.ElecR2Minutes += result.TotalMinutes;
                        break;
                    case "ElectR3":
                        detail.ElecR3Minutes += result.TotalMinutes;
                        break;
                    case "Temp":
                        detail.TempMinutes += result.TotalMinutes;
                        break;
                    case "First Year Apprentice":
                        detail.FirstYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Second Year Apprentice":
                        detail.SecondYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Third Year Apprentice":
                        detail.ThirdYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Fourth Year Apprentice":
                        detail.FourthYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}