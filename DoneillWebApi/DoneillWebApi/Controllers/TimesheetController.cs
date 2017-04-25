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
                Timesheet np = ConvertToTimesheet(p);
                timesheetsForDisplay.Add(np);
            }

            return timesheetsForDisplay;
        }

        public ITimeSheet Get(string identifier)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            ITimeSheet timeSheet = pl.RetrieveTimesheetForIdentifier(identifier);
            Timesheet np = ConvertToTimesheet(timeSheet);

            return np;
        }

        [HttpGet]
        public List<Timesheet> Get(DateTime we)
        {
            IPersistanceLayer pl = new PersistanceLayer();
            List<ITimeSheet> timeSheets = pl.RetrieveTimesheetsForDate(we);

            List<Timesheet> timesheetsForDisplay = new List<Timesheet>();

            foreach (ITimeSheet p in timeSheets)
            {
                Timesheet np = ConvertToTimesheet(p);
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
                TimesheetItem np = ConvertToTimesheetItem(tsi);
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

        private Timesheet ConvertToTimesheet(ITimeSheet ts)
        {
            Timesheet np = new Timesheet();

            np.engineerName = ts.getEngineerName();
            np.weekEndDate = ts.getWeekEndDate();
            np.identifier = ts.getIdentifier();
            np.export = ts.getExport();

            return np;
        }

        private TimesheetItem ConvertToTimesheetItem(ITimeSheetItem tsi)
        {
            TimesheetItem np = new TimesheetItem();
            np.Id = tsi.getIdentifier();
            np.TimesheetId = tsi.getTimesheetIdentifier();
            np.ProjectName = tsi.getProjectName();
            np.Day = tsi.getDay();
            np.dayStartTime = tsi.getStartTime();
            np.dayEndTime = tsi.getEndTime();

            return np;
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
