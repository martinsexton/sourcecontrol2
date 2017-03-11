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
        public List<ITimeSheetItem> getItems()
        {
            throw new NotImplementedException();
        }

        public string getEngineerName()
        {
            throw new NotImplementedException();
        }

        public string getProjectName()
        {
            throw new NotImplementedException();
        }

        public DateTime getWeekEndDate()
        {
            throw new NotImplementedException();
        }
    }
}
