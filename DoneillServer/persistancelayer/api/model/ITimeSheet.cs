using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.api.model
{
    public interface ITimeSheet
    {
        List<ITimeSheetItem> getMondayItems();
        List<ITimeSheetItem> getTuesdayItems();
        List<ITimeSheetItem> getWednesdayItems();
        List<ITimeSheetItem> getThursdayItems();
        List<ITimeSheetItem> getFridayItems();
        List<ITimeSheetItem> getSaturdayItems();
        List<ITimeSheetItem> getSundayItems();
        String getEngineerName();
        DateTime getWeekEndDate();
        String getExport();
        int getIdentifier();
    }
}
