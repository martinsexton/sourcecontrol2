using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationContext _context;

        public RateRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<LabourRate> GetRates()
        {
            return _context.LabourRate.ToList();
        }

        public LabourRate GetRate(string role, DateTime date)
        {
            return _context.LabourRate.Where(r => r.Role.Equals(role) && r.EffectiveFrom <= date && r.EffectiveTo >= date)
                .FirstOrDefault();
        }
    }
}
