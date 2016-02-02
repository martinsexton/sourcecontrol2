using DspPocEncryption;
using DspPocWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DspPocWebApplication.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new EncryptionRequest());
        }

        [HttpPost]
        public ActionResult Index(EncryptionRequest request)
        {
            if (ModelState.IsValid)
            {
                HPSAEncryption encryption = new HPSAEncryption();
                request.EncryptedText = encryption.generateToken(request.DocContract, request.MonthDay, request.Month);

                return View(request);
            }
            else
            {
                return View(request);
            }

        }

        [HttpGet]
        public ActionResult AuthenticateToken(string cust, string hash)
        {
            HPSAEncryption encryption = new HPSAEncryption();

            if (encryption.generateToken(cust).ToUpper().Equals(hash.ToUpper()))
            {
                EncryptionRequest request = new EncryptionRequest();
                request.DocContract = cust;
                request.Month = DateTime.Now.Month;
                request.MonthDay = DateTime.Now.Day;
                request.EncryptedText = hash;

                return View("SecuredPage", request);
            }
            else
            {
                return View("FailedToAuthenticateToken");
            }
        }
    }
}