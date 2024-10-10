using doneillspa.Models;
using System;

namespace doneillspa.Dtos
{
    public class TimesheetReportDto
    {
        public long Id { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileReference { get; set; }
        public string Status { get; set; }
    }
}
