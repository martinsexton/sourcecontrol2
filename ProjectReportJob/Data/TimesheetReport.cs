using System;

namespace ProjectReportJob.Data
{
    public class TimesheetReport
    {
        public long Id { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public String FileReference { get; set; }
        public TimesheetReportStatus Status { get; set; }
    }
}
