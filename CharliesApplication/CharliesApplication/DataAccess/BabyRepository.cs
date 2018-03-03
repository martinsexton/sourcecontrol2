using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharliesApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CharliesApplication.DataAccess
{
    public class BabyRepository : IBabyRepository
    {
        private readonly BabyContext _context;

        public BabyRepository(BabyContext context)
        {
            _context = context;
        }

        public void InsertBaby(Baby b)
        {
            _context.Baby.Add(b);
        }

        public void UpdateBaby(Baby b)
        {
            _context.Entry(b).State = EntityState.Modified;
        }

        public IEnumerable<Baby> GetBabies()
        {
            return _context.Baby.ToList();
        }

        public Baby GetBabyById(long id)
        {
            //Use Include below to eagerly resolve the related BirthDetails entity
            return _context.Baby
                        .Where(b => b.Id == id)
                        .Include(b => b.BirthDetails)
                        .FirstOrDefault();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
