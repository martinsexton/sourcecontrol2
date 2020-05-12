using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface IProjectService
    {
        IEnumerable<LabourRate> GetRates();

    }
}
