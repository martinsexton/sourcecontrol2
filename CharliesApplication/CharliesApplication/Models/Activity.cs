using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharliesApplication.Models
{
    public class Activity
    {
        //Owning Baby
        public Baby Baby { get; set; }

        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public ActivityType Type { get; set; }
    }
}
