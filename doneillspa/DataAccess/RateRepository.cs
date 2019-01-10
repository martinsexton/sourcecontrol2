using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

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

        public void InsertRate(LabourRate r)
        {
            _context.LabourRate.Add(r);
        }

        public void UpdateRate(LabourRate r)
        {
            _context.Entry(r).State = EntityState.Modified;
        }

        public void DeleteRate(LabourRate r)
        {
            _context.Entry(r).State = EntityState.Deleted;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public LabourRate GetRateById(long id)
        {
            return _context.LabourRate
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }
    }
}
