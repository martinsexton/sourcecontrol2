using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            //string userFirstname = ClaimsPrincipal.Current.FindFirst(ClaimTypes.GivenName).Value;
            //ViewBag.Message = String.Format("Welcome {0}", userFirstname);

            return View();
        }

        public ActionResult ListProjects()
        {
            return View();
        }

        public ActionResult NewProject()
        {
            return View();
        }

        //[Authorize(Roles = "admin,engineer")]
        public ActionResult RecordTimesheet()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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