using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace doneillspa.Services
{
    public interface ICertificationService
    {
        IEnumerable<doneillspa.Models.Certification> GetCertificationsByUserId(string userId);
        doneillspa.Models.Certification GetCertificationById(long id);
        void DeleteCertification(long id);
    }
}
