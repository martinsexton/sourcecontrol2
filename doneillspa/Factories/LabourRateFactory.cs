using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Factories
{
    public static class LabourRateFactory
    {
        public static LabourRate CreateLabourRate(DateTime effectiveFrom, DateTime? effectiveTo, double ratePerHour, double overtimeRatePerHour, string role)
        {
            LabourRate rate = new LabourRate();
            rate.EffectiveFrom = effectiveFrom;
            rate.EffectiveTo = effectiveTo;
            rate.RatePerHour = ratePerHour;
            rate.OverTimeRatePerHour = overtimeRatePerHour;
            rate.Role = role;

            return rate;
        }
    }
}
