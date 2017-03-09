using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillServer.Models
{
    public class Project
    {
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public string ContactNumber { get; set; }
        public string Details { get; set; }
    }
}