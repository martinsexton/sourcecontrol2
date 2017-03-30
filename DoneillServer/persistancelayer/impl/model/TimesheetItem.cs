using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.impl.model
{
    class TimesheetItem : ITimeSheetItem
    {
        public int identifier { get; set; }
        public int timesheetIdentifier { get; set; }
        public string dayOfWeek { get; set; }
        public string project { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public int getIdentifier()
        {
            return identifier;
        }

        public int getTimesheetIdentifier()
        {
            return timesheetIdentifier;
        }

        public string getDay()
        {
            return dayOfWeek;
        }

        public string getStartTime()
        {
            return startTime;
        }

        public string getEndTime()
        {
            return endTime;
        }

        public string getProjectName()
        {
            return project;
        }
    }
}
