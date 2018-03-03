using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharliesApplication.Models
{
    public class BirthDetails
    {
        public Baby Baby { get; set; }
        public long BabyId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Hospital { get; set; }
        public decimal Weight { get; set; }
    }
}
