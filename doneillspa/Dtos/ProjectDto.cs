using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class ProjectDto
    {
        public long Id { get; set; }
        public long OwningClientId { get; set; }
        public string Client { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
    }
}
