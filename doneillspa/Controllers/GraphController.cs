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
    public class GraphController : Controller
    {
        private readonly ITimesheetRepository _repository;

        public GraphController(ITimesheetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("api/projecteffort")]
        public IEnumerable<ProjectEffortDto> GetEfforts()
        {
            Dictionary<string, ProjectEffortDto> effort = new Dictionary<string, ProjectEffortDto>();

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets();
            foreach(Timesheet ts in timesheets.AsQueryable())
            {
                foreach(TimesheetEntry tse in ts.TimesheetEntries)
                {
                    if (!effort.ContainsKey(tse.Code))
                    {
                        ProjectEffortDto effortDto = new ProjectEffortDto();
                        effortDto.ProjectName = tse.Code;
                        effortDto.TotalEffort = CalculateDurationInHours(tse);
                        effortDto.TotalCost = CalculateCosts(tse);
                        effort.Add(tse.Code, effortDto);
                    }
                    else
                    {
                        foreach (KeyValuePair<string, ProjectEffortDto> effortDto in effort)
                        {
                            if (effortDto.Key.Equals(tse.Code))
                            {
                                ProjectEffortDto tmp = effortDto.Value;
                                tmp.TotalEffort = tmp.TotalEffort + CalculateDurationInHours(tse);
                            }
                        }
                    }
                }
            }
            List<ProjectEffortDto> dtos = new List<ProjectEffortDto>();
            foreach (KeyValuePair<string, ProjectEffortDto> effortDto in effort)
            {
                dtos.Add(effortDto.Value);
            }

            return dtos;
        }

        private decimal CalculateCosts(TimesheetEntry tse)
        {
            int hrsWorked = CalculateDurationInHours(tse);
            return hrsWorked * 32;
        }

        //Method to determine duration of tse
        private int CalculateDurationInHours(TimesheetEntry tse)
        {
            string[] hrsmins = tse.StartTime.Split(':');
            int startTimehrs = 0;
            int startTimemins = 0;

            Int32.TryParse(hrsmins[0], out startTimehrs);
            Int32.TryParse(hrsmins[1], out startTimemins);

            string[] endTimehrsmins = tse.EndTime.Split(':');
            int endTimehrs = 0;
            int endTimemins = 0;

            Int32.TryParse(endTimehrsmins[0], out endTimehrs);
            Int32.TryParse(endTimehrsmins[1], out endTimemins);

            DateTime start = new DateTime(2018, 1, 1, startTimehrs, startTimemins, 0);
            DateTime end = new DateTime(2018, 1, 1, endTimehrs, endTimemins, 0);

            TimeSpan duration = end - start;

            return duration.Hours;
        }
    }
}