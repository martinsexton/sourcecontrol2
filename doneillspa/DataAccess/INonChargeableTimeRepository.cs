using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface INonChargeableTimeRepository
    {
        IEnumerable<NonChargeableTime> GetNonChargeableTime();
    }
}
