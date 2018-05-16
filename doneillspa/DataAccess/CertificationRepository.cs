using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class CertificationRepository : ICertificationRepository
    {
        private readonly ApplicationContext _context;

        public CertificationRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void DeleteCertification(Certification cert)
        {
            _context.Entry(cert).State = EntityState.Deleted;
        }

        public Certification GetCertificationById(long id)
        {
            return _context.Certification
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }

        public IEnumerable<Certification> GetCertificationsByUserId(string userId)
        {
            return _context.Certification
                        .Where(b => b.UserId.ToString() == userId)
                        .ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
