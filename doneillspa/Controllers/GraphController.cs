using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;
using doneillspa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class GraphController : Controller
    {
        private List<LabourRate> Rates = new List<LabourRate>();

        private readonly ITimesheetService _timesheetService;
        private readonly IProjectService _projectService;

        public GraphController(ITimesheetService tss, IProjectService ps)
        {
            _timesheetService = tss;
            _projectService = ps;

            Rates = _projectService.GetRates().ToList<LabourRate>();
        }

        private double GetRate(string role, DateTime onDate, List<LabourRate> Rates)
        {
            foreach (LabourRate r in Rates)
            {
                if (r.Role.Equals(role) && r.EffectiveFrom <= onDate && (r.EffectiveTo == null || r.EffectiveTo >= onDate))
                {
                    return r.RatePerHour;
                }
            }
            //Default Value
            return 0.0;
        }

        [HttpGet]
        [Route("api/projectcost/{code}")]
        public ProjectCostDto GetProjectCosts(string code)
        {
            ProjectCostDto costs = new ProjectCostDto();
            costs.ProjectName = code;

            IEnumerable<Timesheet> timesheets = _timesheetService.GetTimesheets();
            foreach(Timesheet ts in timesheets.AsQueryable())
            {
                bool weekFound = false;

                if(!(ts.Status == TimesheetStatus.Approved))
                {
                    continue;
                }
                //Gets reset for each timesheet
                decimal projectcost = 0m;

                foreach (TimesheetEntry tse in ts.TimesheetEntries)
                {
                    if (!tse.Code.Equals(code))
                    {
                        continue;
                    }

                    if (!costs.weekAlreadyRecorded(ts.WeekStarting.ToShortDateString()))
                    {
                        weekFound = true;
                        costs.addWeek(ts.WeekStarting.ToShortDateString());
                    }

                    //Increment cost for each timesheet entry in timesheet for given week.
                    projectcost += this.CalculateCosts(tse);
                }

                if (weekFound)
                {
                    costs.addCost(projectcost);
                }
            }

            return costs;
        }

        private decimal CalculateCosts(TimesheetEntry tse)
        {
            int hrsWorked = CalculateDurationInHours(tse);
            double rate = GetRate(tse.Timesheet.Role,DateTime.UtcNow, Rates);
            return hrsWorked * (decimal)rate;
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