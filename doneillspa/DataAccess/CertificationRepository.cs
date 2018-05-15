using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public class CertificationRepository : ICertificationRepository
    {
        private readonly ApplicationContext _context;

        public CertificationRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Certification> GetCertificationsByUserId(string userId)
        {
            return _context.Certification
                        .Where(b => b.UserId.ToString() == userId)
                        .ToList();
        }
    }
}
