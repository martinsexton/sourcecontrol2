using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class Certification
    {
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Expiry { get; set; }
        public string Description { get; set; }

        public bool HasExpired()
        {
            return (Expiry <= DateTime.UtcNow);
        }
    }
}
