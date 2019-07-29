using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services.Certification
{
    public class CertificationService : ICertificationService
    {
        private readonly ICertificationRepository _repository;

        public CertificationService(ICertificationRepository repository)
        {
            _repository = repository;
        }
        public void DeleteCertification(Models.Certification cert)
        {
            _repository.DeleteCertification(cert);
            _repository.Save();
        }

        public Models.Certification GetCertificationById(long id)
        {
            return _repository.GetCertificationById(id);
        }

        public IEnumerable<Models.Certification> GetCertificationsByUserId(string userId)
        {
            return _repository.GetCertificationsByUserId(userId);
        }
    }
}
