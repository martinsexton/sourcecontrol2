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
        public DateTime weekEndDate { get; set; }

        public List<ITimeSheetItem> getItems()
        {
            throw new NotImplementedException();
        }

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
    }
}
