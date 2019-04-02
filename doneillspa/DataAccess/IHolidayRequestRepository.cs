using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface IHolidayRequestRepository
    {
        HolidayRequest GetHolidayRequestById(long id);
        IEnumerable<HolidayRequest> GetHolidayRequestsForApprover(string userId);
        IEnumerable<HolidayRequest> GetHolidayRequestsForUser(string userId);
        void Delete(HolidayRequest request);
        void Save();
    }
}
