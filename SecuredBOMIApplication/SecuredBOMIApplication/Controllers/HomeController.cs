using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SecuredBOMIApplication.Controllers
{
    public class HomeController : Controller
    {
        //App Id URI = https://localhost:44300/ProtectedBomi4
        //Federated Metadata = https://login.windows.net/65d15693-ed84-471e-88e6-b21c16a2af42/FederationMetadata/2007-06/FederationMetadata.xml
        //Client Id = 33abe5d0-db4e-4371-9dab-d8ddfdcb5ec7
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            /*string userfirstname = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName).Value;
            ViewBag.Message = String.Format("Welcome, {0}!", userfirstname);*/

            string userfirstname = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Name).Value;
            ViewBag.Message = String.Format("Welcome, {0}!", userfirstname);

            return View();
        }

        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                            OpenIdConnectAuthenticationDefaults.AuthenticationType,
                            CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}