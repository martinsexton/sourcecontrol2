using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigDay.Models;
using System.Security.Claims;
using WeddingServices.Implementation;
using WeddingServices.Interface;

namespace MartinAndOrlasWeddingApplication.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        IGuestService service;

        public LoginController(IGuestService s)
        {
            service = s;
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginRequest login = new LoginRequest();
            login.ReturnUrl = returnUrl;

            return View(login);
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IGuest guest = service.RetrieveGuestByName(request.Firstname, request.Surname);
            if (guest != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(new List<Claim> { 
                    new Claim(ClaimTypes.Name,guest.getFirstname()),
                    new Claim(ClaimTypes.Surname,guest.getSurname()),
                    new Claim(ClaimTypes.PrimarySid,guest.getIdentifier().ToString()),
                }, "ApplicationCookie");

                var context = Request.GetOwinContext();
                var authManager = context.Authentication;

                authManager.SignIn(identity);
                return Redirect(GetRedirectUrl(request.ReturnUrl));
            }

            ModelState.AddModelError("", "Guest not found!");
            return View();
        }

        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }
            return returnUrl;
        }
    }
}