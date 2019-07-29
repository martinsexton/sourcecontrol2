using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.Services
{
    public interface IHolidayService
    {
        HolidayRequest GetHolidayRequestById(long id);
        IEnumerable<HolidayRequest> GetHolidayRequestsForUser(string userId);
        IEnumerable<HolidayRequest> GetHolidayRequestsForApprover(string userId);

        void Delete(long id);
        void Update(HolidayRequest request);
    }
}
