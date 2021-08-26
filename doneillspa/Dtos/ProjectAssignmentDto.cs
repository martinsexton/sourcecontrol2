using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class ProjectAssignmentDto
    {
        public string Code { get; set; }
        public ICollection<string> Users { get; set; }
    }
}
