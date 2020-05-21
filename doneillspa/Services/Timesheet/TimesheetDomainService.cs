using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services
{
    public static class TimesheetDomainService
    {
        public static LabourWeekDetail BuildLabourWeekDetails(Timesheet ts, List<LabourRate> Rates, string proj)
        {
            //Retrieve details from timesheet and populate the LabourWeekDetail object
            LabourWeekDetail detail = new LabourWeekDetail(proj, ts.WeekStarting);
            Dictionary<string, double> hoursPerDay = RetrieveBreakdownOfHoursPerDay(ts,proj);

            PopulateLabourDetail(ts, Rates, detail, hoursPerDay);

            return detail;
        }

        private static Dictionary<string, double> RetrieveBreakdownOfHoursPerDay(Timesheet ts, string proj)
        {
            Dictionary<string, double> hoursPerDay = new Dictionary<string, double>();
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                if (!tse.Chargeable)
                {
                    continue;
                }
                if (tse.Code.Equals(proj))
                {
                    TimeSpan startTimespan = TimeSpan.Parse(tse.StartTime);
                    TimeSpan endTimespan = TimeSpan.Parse(tse.EndTime);
                    TimeSpan result = endTimespan - startTimespan;

                    if (!hoursPerDay.ContainsKey(tse.Day))
                    {
                        hoursPerDay.Add(tse.Day, result.TotalMinutes);
                    }
                    else
                    {
                        double totalMins = hoursPerDay[tse.Day];
                        totalMins += result.TotalMinutes;
                        hoursPerDay[tse.Day] = totalMins;
                    }
                }
            }
            RemoveLunchBreak(hoursPerDay);

            return hoursPerDay;
        }

        private static void PopulateLabourDetail(Timesheet ts, List<LabourRate> Rates, LabourWeekDetail detail, Dictionary<string, double> hoursPerDay)
        {
            double rate = GetRate(ts, Rates);

            foreach (var item in hoursPerDay)
            {
                double minutesWorked = item.Value;
                switch (ts.Role)
                {
                    case "Administrator":
                        detail.AdministratorCost += ((minutesWorked / 60) * 10);
                        break;
                    case "Supervisor":
                        detail.SupervisorCost += ((minutesWorked / 60) * rate);
                        break;
                    case "ChargeHand":
                        detail.ChargehandCost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR1":
                        detail.ElecR1Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR2":
                        detail.ElecR2Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "ElectR3":
                        detail.ElecR3Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Loc1":
                        detail.Loc1Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Loc2":
                        detail.Loc2Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Loc3":
                        detail.Loc3Cost += ((minutesWorked / 60) * rate);
                        break;
                    case "Temp":
                        detail.TempCost += ((minutesWorked / 60) * rate);
                        break;
                    case "First Year Apprentice":
                        detail.FirstYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Second Year Apprentice":
                        detail.SecondYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Third Year Apprentice":
                        detail.ThirdYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    case "Fourth Year Apprentice":
                        detail.FourthYearApprenticeCost += ((minutesWorked / 60) * rate);
                        break;
                    default:
                        break;
                }
            }
        }

        private static double GetRate(Timesheet ts, List<LabourRate> Rates)
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

        private static void RemoveLunchBreak(Dictionary<string, double> hoursPerDay)
        {
            //Update each of the entries to remove 30 mins for days where engineer worked >= 5 hours
            foreach (string key in hoursPerDay.Keys.ToList())
            {
                double minutesWorked = hoursPerDay[key];
                if (((key.Equals("Sat") || key.Equals("Sun")) && minutesWorked > (5 * 60))
                    || (!(key.Equals("Sat") || key.Equals("Sun")) && minutesWorked >= (5 * 60)))
                {
                    minutesWorked = minutesWorked - 30;
                    hoursPerDay[key] = minutesWorked;
                }
            }
        }
    }
}
