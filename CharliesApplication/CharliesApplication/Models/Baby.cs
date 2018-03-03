using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharliesApplication.Models
{
    public class Baby
    {
        public Baby()
        {
            this.CreatedDate = DateTime.Now;
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public BirthDetails BirthDetails { get; set; }
        private DateTime CreatedDate { get; set; }
    }
}
