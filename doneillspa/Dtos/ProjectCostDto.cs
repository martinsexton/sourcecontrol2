using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class ProjectCostDto
    {
        public ICollection<string> Weeks { get; set; }
        public ICollection<decimal> Costs { get; set; }

        public string ProjectName { get; set; }

       public void addWeek(string week)
        {
            if(Weeks == null)
            {
                Weeks = new List<string>();
            }
            Weeks.Add(week);
        }

        public bool weekAlreadyRecorded(string week)
        {
            if(this.Weeks == null)
            {
                return false;
            }
            return this.Weeks.Contains(week);
        }

        public void addCost(decimal cost)
        {
            if (Costs == null)
            {
                Costs = new List<decimal>();
            }
            Costs.Add(cost);
        }
    }
}
