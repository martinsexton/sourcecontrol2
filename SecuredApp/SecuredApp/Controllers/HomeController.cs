using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public FileStreamResult CreateFile(string exportDetails)
        {
            var byteArray = Encoding.ASCII.GetBytes(exportDetails);
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