using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Email;

namespace doneillspa.Models
{
    public class TimesheetEntry
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public Timesheet Timesheet { get; set; }

        public void PopulateLabourDetail(LabourWeekDetail detail, List<LabourRate> Rates)
        {
            TimeSpan startTimespan = TimeSpan.Parse(StartTime);
            TimeSpan endTimespan = TimeSpan.Parse(EndTime);
            TimeSpan result = endTimespan - startTimespan;

            double rate = GetRate(Timesheet.Role, DateTime.UtcNow, Rates);

            switch (Timesheet.Role)
            {
                case "Administrator":
                    detail.AdministratorCost += ((result.TotalMinutes/60) * 10);
                    break;
                case "Supervisor":
                    detail.SupervisorCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "ChargeHand":
                    detail.ChargehandCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "ElectR1":
                    detail.ElecR1Cost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "ElectR2":
                    detail.ElecR2Cost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "ElectR3":
                    detail.ElecR3Cost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "Temp":
                    detail.TempCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "First Year Apprentice":
                    detail.FirstYearApprenticeCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "Second Year Apprentice":
                    detail.SecondYearApprenticeCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "Third Year Apprentice":
                    detail.ThirdYearApprenticeCost += ((result.TotalMinutes / 60) * rate);
                    break;
                case "Fourth Year Apprentice":
                    detail.FourthYearApprenticeCost += ((result.TotalMinutes / 60) * rate);
                    break;
                default:
                    break;
            }
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
    }
}
