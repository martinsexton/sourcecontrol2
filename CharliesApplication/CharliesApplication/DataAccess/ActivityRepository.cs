using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;

namespace CharliesApplication.DataAccess
{
    public class ActivityRepository : IActivityRepository
    {

        private readonly BabyContext _context;

        public ActivityRepository(BabyContext context)
        {
            _context = context;
        }

        public Activity GetActivityById(long id)
        {
            return _context.Activity
            .Where(a => a.Id == id)
            .FirstOrDefault();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateActivity(Activity ac)
        {
            throw new NotImplementedException();
        }
    }
}
