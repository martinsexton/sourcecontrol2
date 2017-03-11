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
        public DateTime timesheetProject { get; set; }
        public TimesheetItem[] items = new TimesheetItem[5];


        public List<ITimeSheetItem> getItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            items.Add(items[0]);
            items.Add(items[1]);
            items.Add(items[2]);
            items.Add(items[3]);
            items.Add(items[4]);

            return items;
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