using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.impl.model
{
    class Timesheet : ITimeSheet
    {
        public int identifier { get; set; }
        public string engineerName { get; set; }
        public string export { get; set; }
        public DateTime weekEndDate { get; set; }

        public string getEngineerName()
        {
            return engineerName;
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
            throw new NotImplementedException();
        }

        public List<ITimeSheetItem> getTuesdayItems()
        {
            throw new NotImplementedException();
        }

        public List<ITimeSheetItem> getWednesdayItems()
        {
            throw new NotImplementedException();
        }

        public List<ITimeSheetItem> getThursdayItems()
        {
            throw new NotImplementedException();
        }

        public List<ITimeSheetItem> getFridayItems()
        {
            throw new NotImplementedException();
        }


        public List<ITimeSheetItem> getSaturdayItems()
        {
            throw new NotImplementedException();
        }

        public List<ITimeSheetItem> getSundayItems()
        {
            throw new NotImplementedException();
        }


        public string getExport()
        {
            return export;
        }
    }
}
