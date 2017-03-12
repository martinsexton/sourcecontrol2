using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillWebApi.Models
{
    public class Timesheet : ITimeSheet
    {
        public string engineerName { get; set; }
        public DateTime weekEndDate { get; set; }
        public TimesheetItem[] items = new TimesheetItem[5];


        public List<ITimeSheetItem> getItems()
        {
            List<ITimeSheetItem> items2 = new List<ITimeSheetItem>();
            items2.Add(items[0]);
            items2.Add(items[1]);
            items2.Add(items[2]);
            items2.Add(items[3]);
            items2.Add(items[4]);

            return items2;
        }

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
    }
}