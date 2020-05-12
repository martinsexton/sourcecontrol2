using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class LabourRateDto
    {
        public long Id { get; set; }
        public double RatePerHour { get; set; }
        public double OverTimeRatePerHour { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Role { get; set; }
    }
}
