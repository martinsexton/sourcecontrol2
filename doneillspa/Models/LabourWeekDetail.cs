using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class LabourWeekDetail
    {
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
                return (SupervisorMinutes / 60) * 37.56;
            }
        }

        public double ChargehandMinutes { get; set; }
        public double ChargehandCost
        {
            get
            {
                return (ChargehandMinutes/60) * 35.1;
            }
        }

        public double ElecR1Minutes { get; set; }
        public double ElecR1Cost
        {
            get
            {
                return (ElecR1Minutes / 60) * 37.94;
            }
        }

        public double ElecR2Minutes { get; set; }
        public double ElecR2Cost
        {
            get
            {
                return (ElecR2Minutes / 60) * 34.22;
            }
        }

        public double ElecR3Minutes { get; set; }
        public double ElecR3Cost
        {
            get
            {
                return (ElecR3Minutes / 60) * 33.03;
            }
        }

        public double TempMinutes { get; set; }
        public double TempCost
        {
            get
            {
                return (TempMinutes / 60) * 14.13;
            }
        }

        public double FirstYearApprenticeMinutes { get; set; }
        public double FirstYearApprenticeCost
        {
            get
            {
                return (FirstYearApprenticeMinutes / 60) * 9.7;
            }
        }

        public double SecondYearApprenticeMinutes { get; set; }
        public double SecondYearApprenticeCost
        {
            get
            {
                return (SecondYearApprenticeMinutes / 60) * 14.85;
            }
        }

        public double ThirdYearApprenticeMinutes { get; set; }
        public double ThirdYearApprenticeCost
        {
            get
            {
                return (ThirdYearApprenticeMinutes / 60) * 21.46;
            }
        }

        public double FourthYearApprenticeMinutes { get; set; }
        public double FourthYearApprenticeCost
        {
            get
            {
                return (FourthYearApprenticeMinutes / 60) * 26.41;
            }
        }
    }
}
