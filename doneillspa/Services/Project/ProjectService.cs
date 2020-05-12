using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Factories;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRateRepository _rateRepository;

        public ProjectService(IRateRepository rr)
        {
            _rateRepository = rr;
        }

        public IEnumerable<LabourRate> GetRates()
        {
            return _rateRepository.GetRates();
        }
    }
}
