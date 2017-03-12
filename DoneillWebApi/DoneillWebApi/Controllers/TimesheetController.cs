using DoneillWebApi.Models;
using persistancelayer;
using persistancelayer.api;
using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DoneillWebApi.Controllers
{
    public class TimesheetController : ApiController
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Timesheet> Get()
        {
            IPersistanceLayer pl = new PersistanceLayer();
            List<ITimeSheet> timeSheets = pl.RetrieveTimesheets();
            List<Timesheet> timesheetsForDisplay = new List<Timesheet>();

            foreach (ITimeSheet p in timeSheets)
            {
                Timesheet np = new Timesheet();

                np.engineerName = p.getEngineerName();
                np.weekEndDate = p.getWeekEndDate();
                np.identifier = p.getIdentifier();


                timesheetsForDisplay.Add(np);
            }

            return timesheetsForDisplay;
        }

        // GET api/values/5
        [HttpGet]
        public List<TimesheetItem> Get(int id)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            List<ITimeSheetItem> timesheetItems = pl.RetrieveTimesheetItems(id);
            List<TimesheetItem> timesheetItemsForDisplay = new List<TimesheetItem>();

            foreach (ITimeSheetItem tsi in timesheetItems)
            {
                TimesheetItem np = new TimesheetItem();
                np.Id = tsi.getIdentifier();
                np.TimesheetId = tsi.getTimesheetIdentifier();
                np.ProjectName = tsi.getProjectName();
                np.Day = tsi.getDay();
                np.dayStartTime = tsi.getStartTime();
                np.dayEndTime = tsi.getEndTime();

                timesheetItemsForDisplay.Add(np);
            }

            return timesheetItemsForDisplay;
        }

        // POST api/values
        [HttpPost]
        public void Post(Timesheet value)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            pl.CreateTimesheet(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
