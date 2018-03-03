using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;

namespace CharliesApplication.DataAccess
{
    public interface IBabyRepository
    {
        void InsertBaby(Baby b);
        void UpdateBaby(Baby b);
        IEnumerable<Baby> GetBabies();
        Baby GetBabyById(long id);
        void Save();
    }
}
