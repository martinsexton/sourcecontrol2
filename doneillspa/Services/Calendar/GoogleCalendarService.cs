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
    public class GoogleCalendarService : ICalendarService
    {
        private IConfiguration Configuration;

        public GoogleCalendarService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void CreateEvent(DateTime fromDate, int days, string description)
        {
            var service = CreateCalendarService();
            var InsertRequest = BuildEventInsertRequest(service, fromDate, days, description);
            InsertRequest.Execute();
        }

        private EventsResource.InsertRequest BuildEventInsertRequest(CalendarService service, DateTime fromDate, int days, string description)
        {
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

            return service.Events.Insert(myEvent, Configuration["calendar:id"]);
        }

        private CalendarService CreateCalendarService()
        {
            var clientEmail = Configuration["calendar:client_email"];

            //NB: ensure you remove any \n in the private key before adding to azure configuration page. 
            //i.e. take the private key from json file
            //but then strip out any \n characters before adding to azure configuration.
            string privatekey = Configuration["calendar:private_key"];
            string privateKeyToUse = "-----BEGIN PRIVATE KEY-----" + privatekey + "-----END PRIVATE KEY-----";
            string calanderId = Configuration["calendar:id"];

            string[] Scopes = { CalendarService.Scope.Calendar };

            ServiceAccountCredential credential;

            credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(clientEmail)
            {
                Scopes = Scopes
            }.FromPrivateKey(privateKeyToUse));

            return new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Doneill Calendar Application",
            });
        }
    }
}
