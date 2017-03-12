using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.api.model
{
    public interface ITimeSheetItem
    {
        int getIdentifier();
        int getTimesheetIdentifier();
        String getDay();
        String getStartTime();
        String getEndTime();
        String getProjectName();
    }
}
