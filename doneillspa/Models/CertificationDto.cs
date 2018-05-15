using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Models
{
    public class CertificationDto
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Expiry { get; set; }
        public string Description { get; set; }
    }
}
