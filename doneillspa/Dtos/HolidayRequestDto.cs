using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos
{
    public class HolidayRequestDto
    {
        public long Id { get; set; }
        public DateTime Fromdate { get; set; }
        public int Days { get; set; }
        public string ApproverId { get; set; }
    }
}
