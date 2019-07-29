using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class CertificationService : ICertificationService
    {
        private readonly ICertificationRepository _repository;

        public CertificationService(ICertificationRepository repository)
        {
            _repository = repository;
        }
        public void DeleteCertification(long id)
        {
            Certification cert = GetCertificationById(id);
            _repository.DeleteCertification(cert);
            _repository.Save();
        }

        public Certification GetCertificationById(long id)
        {
            return _repository.GetCertificationById(id);
        }

        public IEnumerable<Certification> GetCertificationsByUserId(string userId)
        {
            return _repository.GetCertificationsByUserId(userId);
        }
    }
}
