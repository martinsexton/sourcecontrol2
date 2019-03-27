using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class LabourWeekDetail
    {
        public DateTime Week { get; set; }

        public double AdministratorCost { get; set; }

        public double SupervisorCost { get; set; }

        public double ChargehandCost { get; set; }

        public double ElecR1Cost { get; set; }

        public double ElecR2Cost { get; set; }

        public double ElecR3Cost { get; set; }

        public double TempCost { get; set; }

        public double FirstYearApprenticeCost { get; set; }

        public double SecondYearApprenticeCost { get; set; }

        public double ThirdYearApprenticeCost { get; set; }

        public double FourthYearApprenticeCost { get; set; }

        public double TotalCost
        {
            get
            {
                return this.SupervisorCost + this.ChargehandCost + this.ElecR1Cost + this.ElecR2Cost + this.ElecR3Cost + this.FirstYearApprenticeCost + this.SecondYearApprenticeCost + this.ThirdYearApprenticeCost + this.FourthYearApprenticeCost;
            }
        }
    }
}
