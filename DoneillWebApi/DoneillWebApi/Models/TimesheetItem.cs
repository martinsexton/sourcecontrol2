using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoneillWebApi.Models
{
    public class TimesheetItem : ITimeSheetItem
    {
        public string Day { get; set; }
        public DateTime dayStartTime { get; set; }
        public DateTime dayEndTime { get; set; }

        public string getDay()
        {
            throw new NotImplementedException();
        }

        public string getStartTime()
        {
            throw new NotImplementedException();
        }

        public string getEndTime()
        {
            throw new NotImplementedException();
        }
    }
}