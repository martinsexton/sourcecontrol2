using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharliesApplication.Models
{
    public class Appointment
    {
        //Owning Baby
        public Baby Baby { get; set; }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Outcome { get; set; }
    }
}
