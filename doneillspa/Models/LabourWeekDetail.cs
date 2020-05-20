using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class LabourWeekDetail
    {
        public LabourWeekDetail(string project, DateTime weekStarting)
        {
            this.Project = project;
            this.Week = weekStarting;
        }

        public DateTime Week { get; set; }

        public string Project { get; set; }

        public double AdministratorCost { get; set; }

        public double SupervisorCost { get; set; }

        public double ChargehandCost { get; set; }

        public double ElecR1Cost { get; set; }

        public double ElecR2Cost { get; set; }

        public double ElecR3Cost { get; set; }

        public double Loc1Cost { get; set; }

        public double Loc2Cost { get; set; }

        public double Loc3Cost { get; set; }

        public double TempCost { get; set; }

        public double FirstYearApprenticeCost { get; set; }

        public double SecondYearApprenticeCost { get; set; }

        public double ThirdYearApprenticeCost { get; set; }

        public double FourthYearApprenticeCost { get; set; }

        public void ammendDetails(LabourWeekDetail week)
        {
            if(week.TotalCost > 0)
            {
                //increase totals based on the labourWeekDetail provided
                this.SupervisorCost += week.SupervisorCost;
                this.AdministratorCost += week.AdministratorCost;
                this.ChargehandCost += week.ChargehandCost;
                this.ElecR1Cost += week.ElecR1Cost;
                this.ElecR2Cost += week.ElecR2Cost;
                this.ElecR3Cost += week.ElecR3Cost;
                this.Loc1Cost += week.Loc1Cost;
                this.Loc2Cost += week.Loc2Cost;
                this.Loc3Cost += week.Loc3Cost;
                this.TempCost += week.TempCost;
                this.FirstYearApprenticeCost += week.FirstYearApprenticeCost;
                this.SecondYearApprenticeCost += week.SecondYearApprenticeCost;
                this.ThirdYearApprenticeCost += week.ThirdYearApprenticeCost;
                this.FourthYearApprenticeCost += week.FourthYearApprenticeCost;
            }
        }

        public double TotalCost
        {
            get
            {
                return this.SupervisorCost + this.ChargehandCost + this.ElecR1Cost + this.ElecR2Cost + this.ElecR3Cost + this.Loc1Cost + this.Loc2Cost + this.Loc3Cost + this.FirstYearApprenticeCost + this.SecondYearApprenticeCost + this.ThirdYearApprenticeCost + this.FourthYearApprenticeCost;
            }
        }
    }
}
