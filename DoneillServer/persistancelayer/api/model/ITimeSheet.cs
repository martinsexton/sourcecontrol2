using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.api.model
{
    public interface ITimeSheet
    {
        List<ITimeSheetItem> getItems();
        String getEngineerName();
        DateTime getWeekEndDate();
        int getIdentifier();
    }
}
