using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class LabourWeekDetail
    {
        public List<LabourRate> Rates;

        public LabourWeekDetail(List<LabourRate> rates)
        {
            this.Rates = rates;
        }

        private double GetRate(string role, DateTime onDate)
        {
            foreach(LabourRate r in Rates)
            {
                if (r.Role.Equals(role) && r.EffectiveFrom <= onDate && (r.EffectiveTo == null || r.EffectiveTo >= onDate))
                {
                    return r.RatePerHour;
                }
            }
            //Default Value
            return 0.0;
        }

        public DateTime Week { get; set; }
        public double AdministratorMinutes { get; set; }
        public double AdministratorCost
        {
            get
            {
                return (AdministratorMinutes / 60) * 10;
            }
        }

        public double SupervisorMinutes { get; set; }
        public double SupervisorCost
        {
            get
            {
                return (SupervisorMinutes / 60) * GetRate("Supervisor", this.Week); ;
            }
        }

        public double ChargehandMinutes { get; set; }
        public double ChargehandCost
        {
            get
            {
                return (ChargehandMinutes/60) * GetRate("ChargeHand", this.Week); ;
            }
        }

        public double ElecR1Minutes { get; set; }
        public double ElecR1Cost
        {
            get
            {
                return (ElecR1Minutes / 60) * GetRate("ElectR1", this.Week); ;
            }
        }

        public double ElecR2Minutes { get; set; }
        public double ElecR2Cost
        {
            get
            {
                return (ElecR2Minutes / 60) * GetRate("ElectR2", this.Week); ;
            }
        }

        public double ElecR3Minutes { get; set; }
        public double ElecR3Cost
        {
            get
            {
                return (ElecR3Minutes / 60) * GetRate("ElectR3", this.Week); ;
            }
        }

        public double TempMinutes { get; set; }
        public double TempCost
        {
            get
            {
                return (TempMinutes / 60) * GetRate("Temp", this.Week); ;
            }
        }

        public double FirstYearApprenticeMinutes { get; set; }
        public double FirstYearApprenticeCost
        {
            get
            {
                return (FirstYearApprenticeMinutes / 60) * GetRate("First Year Apprentice", this.Week); ;
            }
        }

        public double SecondYearApprenticeMinutes { get; set; }
        public double SecondYearApprenticeCost
        {
            get
            {
                return (SecondYearApprenticeMinutes / 60) * GetRate("Second Year Apprentice", this.Week); ;
            }
        }

        public double ThirdYearApprenticeMinutes { get; set; }
        public double ThirdYearApprenticeCost
        {
            get
            {
                return (ThirdYearApprenticeMinutes / 60) * GetRate("Third Year Apprentice", this.Week);
            }
        }

        public double FourthYearApprenticeMinutes { get; set; }
        public double FourthYearApprenticeCost
        {
            get
            {
                return (FourthYearApprenticeMinutes / 60) * GetRate("Fourth Year Apprentice", this.Week);
            }
        }

        public double TotalCost
        {
            get
            {
                return this.SupervisorCost + this.ChargehandCost + this.ElecR1Cost + this.ElecR2Cost + this.ElecR3Cost + this.FirstYearApprenticeCost + this.SecondYearApprenticeCost + this.ThirdYearApprenticeCost + this.FourthYearApprenticeCost;
            }
        }
    }
}
