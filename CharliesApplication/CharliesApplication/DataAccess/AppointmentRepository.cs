using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CharliesApplication.DataAccess
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly BabyContext _context;

        public AppointmentRepository(BabyContext context)
        {
            _context = context;
        }
        public Appointment GetAppointmentById(long id)
        {
            return _context.Appointment
                        .Where(a => a.Id == id)
                        .Include(a => a.Baby)
                        .FirstOrDefault();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateAppointment(Appointment b)
        {
            throw new NotImplementedException();
        }
    }
}
