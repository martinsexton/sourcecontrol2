using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class HolidayRequestRepository : IHolidayRequestRepository
    {
        private readonly ApplicationContext _context;

        public HolidayRequestRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<HolidayRequest> GetHolidayRequestsForApprover(string userId)
        {
            return _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Where(b => b.Approver.Id.ToString() == userId)
                        .ToList();
        }

        public IEnumerable<HolidayRequest> GetHolidayRequestsForUser(string userId)
        {
            return _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Where(b => b.UserId.ToString() == userId)
                        .ToList();
        }
    }
}
