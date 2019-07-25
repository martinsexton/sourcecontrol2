using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public class NonChargeableTimeRepository : INonChargeableTimeRepository
    {
        private readonly ApplicationContext _context;

        public NonChargeableTimeRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<NonChargeableTime> GetNonChargeableTime()
        {
            return _context.NonChargeableTime.ToList();
        }
    }
}
