using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface IHolidayService
    {
        IEnumerable<HolidayRequest> GetHolidayRequestsForUser(string userId);
        IEnumerable<HolidayRequest> GetHolidayRequestsForApprover(string userId);
    }
}
