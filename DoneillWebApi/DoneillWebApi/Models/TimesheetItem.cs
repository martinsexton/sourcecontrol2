using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillWebApi.Models
{
    public class TimesheetItem : ITimeSheetItem
    {
        public string Day { get; set; }
        public string ProjectName { get; set; }
        public string dayStartTime { get; set; }
        public string dayEndTime { get; set; }

        public string getDay()
        {
            return Day;
        }

        public string getStartTime()
        {
            return dayStartTime;
        }

        public string getEndTime()
        {
            return dayEndTime;
        }


        public string getProjectName()
        {
            return ProjectName;
        }
    }
}