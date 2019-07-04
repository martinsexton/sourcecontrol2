using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface IRateRepository
    {
        LabourRate GetRate(string role, DateTime date);
        IEnumerable<LabourRate> GetRates();
        LabourRate GetRateById(long id);
        long InsertRate(LabourRate r);
        void UpdateRate(LabourRate r);
        void DeleteRate(LabourRate r);
    }
}
