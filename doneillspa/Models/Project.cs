using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Project
    {
        public long Id { get; set; }
        public long OwningClientId { get; set; }
        public virtual Client OwningClient { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
    }
}
