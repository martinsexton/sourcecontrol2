using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public abstract class Notification
    {
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public long Id { get; set; }
        public int Type { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
