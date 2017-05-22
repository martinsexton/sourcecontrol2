using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using SecuredApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SecuredApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = "admin")]
        public ActionResult Projects()
        {
            return View();
        }

        public ActionResult Employees()
        {
            return View();
        }

        [ValidateInput(false)]
        public FileStreamResult CreateFile(string weekEnding)
        {
            List<Timesheet> data = null;

            String unescapedString = weekEnding.Substring(1, weekEnding.Length - 2);

            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://doneillwebapi.azurewebsites.net");
                client.BaseAddress = new Uri("http://localhost:49320");
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                HttpResponseMessage response = client.GetAsync("/api/timesheet?we=" + unescapedString).Result;
                string stringData = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<List<Timesheet>>(stringData);
            }
            StringBuilder responseXml = new StringBuilder();
            responseXml.Append("<Timesheets>");

            foreach (Timesheet t in data)
            {
                responseXml.Append(t.getExport());
            }

            responseXml.Append("</Timesheets>");
            var byteArray = Encoding.ASCII.GetBytes(responseXml.ToString());
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "export.xml");
        }

        public ActionResult ListTimesheets()
        {
            return View();
        }

        public ActionResult SearchTimesheets()
        {
            return View();
        }

        //[Authorize(Roles = "engineer")]
        public ActionResult RecordTimesheet()
        {
            return View();
        }

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void Signout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SuccessfulSignout()
        {
            return View();
        }
    }
}