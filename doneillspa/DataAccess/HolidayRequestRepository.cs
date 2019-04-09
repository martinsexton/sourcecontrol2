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

        public void Delete(HolidayRequest request)
        {
            _context.Entry(request).State = EntityState.Deleted; ;
        }

        public HolidayRequest GetHolidayRequestById(long id)
        {
            return _context.HolidayRequest
                        .Include(b => b.Approver)
                        .Include(b => b.User)
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
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

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
