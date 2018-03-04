using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;

namespace CharliesApplication.DataAccess
{
    public interface IActivityRepository
    {
        void UpdateActivity(Activity ac);
        Activity GetActivityById(long id);
        void Save();
    }
}
