using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectReportJob.Data
{
    public class TimesheetEntry
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Code { get; set; }
        public string Details { get; set; }
        public bool Chargeable { get; set; }
        public Timesheet Timesheet { get; set; }

        public bool IsEntryChargeable()
        {
            if (Code.Equals("NC4") || Code.Equals("NC5") || Code.Equals("NC6"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int DurationInHours()
        {
            string[] hrsmins = StartTime.Split(':');
            int startTimehrs = 0;
            int startTimemins = 0;

            Int32.TryParse(hrsmins[0], out startTimehrs);
            Int32.TryParse(hrsmins[1], out startTimemins);

            string[] endTimehrsmins = EndTime.Split(':');
            int endTimehrs = 0;
            int endTimemins = 0;

            Int32.TryParse(endTimehrsmins[0], out endTimehrs);
            Int32.TryParse(endTimehrsmins[1], out endTimemins);

            DateTime start = new DateTime(2018, 1, 1, startTimehrs, startTimemins, 0);
            DateTime end = new DateTime(2018, 1, 1, endTimehrs, endTimemins, 0);

            TimeSpan duration = end - start;

            return duration.Hours;
        }
    }
}
