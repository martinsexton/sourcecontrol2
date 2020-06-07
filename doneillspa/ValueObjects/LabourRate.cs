using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.ValueObjects
{
    public class LabourRate
    {
        public long Id { get; set; }
        public double RatePerHour { get; set; }
        public double OverTimeRatePerHour { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Role { get; set; }

        private LabourRate() { }

        public LabourRate(DateTime effectiveFrom, DateTime? effectiveTo, double ratePerHour, double overtimeRatePerHour, string role)
        {
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
            RatePerHour = ratePerHour;
            OverTimeRatePerHour = overtimeRatePerHour;
            Role = role;
        }
    }
}
