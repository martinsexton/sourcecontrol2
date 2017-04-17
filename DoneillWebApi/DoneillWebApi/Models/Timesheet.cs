using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillWebApi.Models
{
    public class Timesheet : ITimeSheet
    {
        public int identifier { get; set; }
        public string engineerName { get; set; }
        public DateTime weekEndDate { get; set; }
        public string export { get; set; }
        public TimesheetItem[] mondayItems { get; set; }
        public TimesheetItem[] tuesdayItems { get; set; }
        public TimesheetItem[] wednesdayItems { get; set; }
        public TimesheetItem[] thursdayItems { get; set; }
        public TimesheetItem[] fridayItems { get; set; }
        public TimesheetItem[] saturdayItems { get; set; }
        public TimesheetItem[] sundayItems { get; set; }


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

        public List<ITimeSheetItem> getMondayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < mondayItems.Length; i++)
            {
                items.Add(mondayItems[i]);
            }

            return items;
        }

        public List<ITimeSheetItem> getTuesdayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < tuesdayItems.Length; i++)
            {
                items.Add(tuesdayItems[i]);
            }

            return items;
        }

        public List<ITimeSheetItem> getWednesdayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < wednesdayItems.Length; i++)
            {
                items.Add(wednesdayItems[i]);
            }

            return items;
        }

        public List<ITimeSheetItem> getThursdayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < thursdayItems.Length; i++)
            {
                items.Add(thursdayItems[i]);
            }

            return items;
        }

        public List<ITimeSheetItem> getFridayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < fridayItems.Length; i++)
            {
                items.Add(fridayItems[i]);
            }

            return items;
        }


        public List<ITimeSheetItem> getSaturdayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < saturdayItems.Length; i++)
            {
                items.Add(saturdayItems[i]);
            }

            return items;
        }

        public List<ITimeSheetItem> getSundayItems()
        {
            List<ITimeSheetItem> items = new List<ITimeSheetItem>();
            for (int i = 0; i < sundayItems.Length; i++)
            {
                items.Add(sundayItems[i]);
            }

            return items;
        }

        public string getExport()
        {
            return export;
        }
    }
}