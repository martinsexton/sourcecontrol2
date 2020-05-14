using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.DataAccess;
using doneillspa.Models;

namespace doneillspa.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRequestRepository _holidayRepository;

        public HolidayService(IHolidayRequestRepository repository)
        {
            _holidayRepository = repository;
        }

        public IEnumerable<HolidayRequest> GetHolidayRequestsForApprover(string userId)
        {
            return _holidayRepository.GetHolidayRequestsForApprover(userId);
        }

        public IEnumerable<HolidayRequest> GetHolidayRequestsForUser(string userId)
        {
            return _holidayRepository.GetHolidayRequestsForUser(userId);
        }
    }
}
