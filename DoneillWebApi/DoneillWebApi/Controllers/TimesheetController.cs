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
        public Timesheet Get(int id)
        {
            throw new NotImplementedException();
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
