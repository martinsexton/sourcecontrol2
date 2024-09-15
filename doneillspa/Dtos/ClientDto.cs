using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class ClientDto
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProjectDto> Projects { get; set; }
    }
}
