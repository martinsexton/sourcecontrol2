using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace doneillspa.Services.Calendar
{
    public class DoneillCalendarService : ICalendarService
    {
        private IConfiguration Configuration;

        public DoneillCalendarService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void CreateEvent(DateTime fromDate, int days, string description)
        {
            var clientEmail = Configuration["calendar:client_email"];
            var privatekey = Configuration["calendar:private_key"];
            string calanderId = Configuration["calendar:id"];

            string[] Scopes = { CalendarService.Scope.Calendar };

            ServiceAccountCredential credential;

            credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(clientEmail)
            {
                Scopes = Scopes
            }.FromPrivateKey(privatekey));

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Doneill Calendar Application",
            });

            var calendar = service.Calendars.Get(calanderId).Execute();

            var myEvent = new Event
            {
                Summary = description,
                Location = "Doneill Offices",
                Description = "Holidays to be taken",
                Start = new EventDateTime()
                {
                    DateTime = fromDate,
                    TimeZone = "Europe/Dublin",
                },
                End = new EventDateTime()
                {
                    DateTime = fromDate.AddDays(days),
                    TimeZone = "Europe/Dublin",
                },
            };

            var InsertRequest = service.Events.Insert(myEvent, calanderId);

            try
            {
                InsertRequest.Execute();
            }
            catch (Exception ex)
            {
                //What to do here        
            }
        }
    }
}
