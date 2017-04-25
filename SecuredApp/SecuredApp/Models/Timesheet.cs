using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecuredApp.Models
{
    public class Timesheet
    {
        public int identifier { get; set; }
        public string engineerName { get; set; }
        public DateTime weekEndDate { get; set; }
        public string export { get; set; }


        public string getEngineerName()
        {
            return engineerName;
        }

        public string getProjectName()
        {
            return getProjectName();
        }


        public DateTime getWeekEndDate()
        {
            return weekEndDate;
        }


        public int getIdentifier()
        {
            return identifier;
        }

        public string getExport()
        {
            return export;
        }
    }
}