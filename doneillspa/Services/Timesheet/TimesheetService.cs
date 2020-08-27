using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Factories;
using doneillspa.Helpers;
using doneillspa.Models;
using doneillspa.Specifications;
using doneillspa.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace doneillspa.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetService(ITimesheetRepository tsr, UserManager<ApplicationUser> userManager)
        {
            _repository = tsr;
            _userManager = userManager;
        }

        public void RecordAnnualLeave(string userName, DateTime start, int numberOfDays)
        {
            ApplicationUser userToVerify = _userManager.FindByNameAsync(userName.ToUpper()).Result;
            string role = _userManager.GetRolesAsync(userToVerify).Result.FirstOrDefault();

            List<DateTime> startOfWeeks = new List<DateTime>();
            List<Timesheet> timesheets = new List<Timesheet>();
            Timesheet timesheet = null;

            for (int i = 0; i < numberOfDays; i++)
            {
                DateTime startOfWeek = GetFirstDayOfWeek(start);

                if (!startOfWeeks.Contains(startOfWeek)){
                    startOfWeeks.Add(startOfWeek);

                    timesheet = GetOrCreateTimesheet(userName, startOfWeek, role, userToVerify);
                    if (!timesheets.Contains(timesheet))
                    {
                        timesheets.Add(timesheet);
                    }
                }
                if(timesheet != null)
                {
                    if (!start.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        timesheet.RecordAnnualLeaveForDay(start.DayOfWeek);
                    }
                    //Increment date
                    start = start.AddDays(1);
                }
            }

            SaveTimesheetEntries(timesheets);
        }

        private Timesheet GetOrCreateTimesheet(string userName, DateTime startOfWeek, string role, ApplicationUser userToVerify)
        {
            Timesheet timesheet = _repository.GetTimesheetsByUser(userName).Where(ts => ts.WeekStarting.Date.Equals(startOfWeek.Date)).FirstOrDefault();
            if (timesheet == null)
            {
                timesheet = TimesheetFactory.CreateTimesheet(userName, startOfWeek, role, userToVerify.Id);
                _repository.InsertTimesheet(timesheet);
            }
            return timesheet;
        }

        public ProjectCostDto DetermineProjectCosts(string code)
        {
            ProjectCostDto costs = new ProjectCostDto();
            costs.ProjectName = code;

            ApprovedTimesheetForProject approvedTimesheetSpecification = new ApprovedTimesheetForProject(code);

            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets();
            foreach (Timesheet ts in timesheets.AsQueryable())
            {
                if (approvedTimesheetSpecification.IsSatisfied(ts) && !costs.weekAlreadyRecorded(ts.WeekStarting.ToShortDateString()))
                {
                    double rate = _repository.GetRateForTimesheet(ts);
                    costs.addWeek(ts.WeekStarting.ToShortDateString());
                    costs.addCost(ts.CalculateHoursWorkedForProject(code) * (decimal)rate);
                }
            }

            return costs;
        }

        public LabourWeekDetail BuildLabourWeekDetails(Timesheet ts, List<LabourRate> Rates, string proj)
        {
            //Retrieve details from timesheet and populate the LabourWeekDetail object
            LabourWeekDetail detail = new LabourWeekDetail(proj, ts.WeekStarting);
            Dictionary<string, double> hoursPerDay = RetrieveBreakdownOfHoursPerDay(ts, proj);

            PopulateLabourDetail(ts, Rates, detail, hoursPerDay);

            return detail;
        }


        private Dictionary<string, double> RetrieveBreakdownOfHoursPerDay(Timesheet ts, string proj)
        {
            ChargeableTseForProject chargeableTseSpecification = new ChargeableTseForProject(proj);

            Dictionary<string, double> hoursPerDay = new Dictionary<string, double>();
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                //If the specification is satisfied then proceed
                if (chargeableTseSpecification.IsSatisfied(tse))
                {
                    if (!hoursPerDay.ContainsKey(tse.Day))
                    {
                        hoursPerDay.Add(tse.Day, tse.DurationInHours());
                    }
                    else
                    {
                        double totalHours = hoursPerDay[tse.Day];
                        totalHours += tse.DurationInHours();
                        hoursPerDay[tse.Day] = totalHours;
                    }
                }
            }
            RemoveLunchBreak(hoursPerDay);

            return hoursPerDay;
        }

        private void PopulateLabourDetail(Timesheet ts, List<LabourRate> Rates, LabourWeekDetail detail, Dictionary<string, double> hoursPerDay)
        {
            double rate = GetRate(ts, Rates);

            foreach (var item in hoursPerDay)
            {
                double hoursWorked = item.Value;
                switch (ts.Role)
                {
                    case "Administrator":
                        detail.AdministratorCost += ((hoursWorked) * 10);
                        break;
                    case "Supervisor":
                        detail.SupervisorCost += ((hoursWorked) * rate);
                        break;
                    case "ChargeHand":
                        detail.ChargehandCost += ((hoursWorked) * rate);
                        break;
                    case "ElectR1":
                        detail.ElecR1Cost += ((hoursWorked) * rate);
                        break;
                    case "ElectR2":
                        detail.ElecR2Cost += ((hoursWorked) * rate);
                        break;
                    case "ElectR3":
                        detail.ElecR3Cost += ((hoursWorked) * rate);
                        break;
                    case "Loc1":
                        detail.Loc1Cost += ((hoursWorked) * rate);
                        break;
                    case "Loc2":
                        detail.Loc2Cost += ((hoursWorked) * rate);
                        break;
                    case "Loc3":
                        detail.Loc3Cost += ((hoursWorked) * rate);
                        break;
                    case "Temp":
                        detail.TempCost += ((hoursWorked) * rate);
                        break;
                    case "First Year Apprentice":
                        detail.FirstYearApprenticeCost += ((hoursWorked) * rate);
                        break;
                    case "Second Year Apprentice":
                        detail.SecondYearApprenticeCost += ((hoursWorked) * rate);
                        break;
                    case "Third Year Apprentice":
                        detail.ThirdYearApprenticeCost += ((hoursWorked) * rate);
                        break;
                    case "Fourth Year Apprentice":
                        detail.FourthYearApprenticeCost += ((hoursWorked) * rate);
                        break;
                    default:
                        break;
                }
            }
        }

        private double GetRate(Timesheet ts, List<LabourRate> Rates)
        {
            foreach (LabourRate r in Rates)
            {
                if (r.Role.Equals(ts.Role) && r.EffectiveFrom <= ts.WeekStarting.Date && (r.EffectiveTo == null || r.EffectiveTo >= ts.WeekStarting.Date))
                {
                    return r.RatePerHour;
                }
            }
            //Default Value
            return 0.0;
        }

        private void RemoveLunchBreak(Dictionary<string, double> hoursPerDay)
        {
            //Update each of the entries to remove 30 mins for days where engineer worked >= 5 hours
            foreach (string key in hoursPerDay.Keys.ToList())
            {
                double hoursWorked = hoursPerDay[key];
                if (((key.Equals("Sat") || key.Equals("Sun")) && hoursWorked > (5))
                    || (!(key.Equals("Sat") || key.Equals("Sun")) && hoursWorked >= (5)))
                {
                    hoursWorked = hoursWorked - 0.5;
                    hoursPerDay[key] = hoursWorked;
                }
            }
        }

        private void SaveTimesheetEntries(List<Timesheet> timesheets)
        {
            foreach (Timesheet ts in timesheets)
            {
                _repository.UpdateTimesheet(ts);
            }
        }

        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != DayOfWeek.Monday)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
    }
}
