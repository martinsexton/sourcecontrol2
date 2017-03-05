using SignOnMonitor.Models;
using SignOnRepository.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignOnMonitor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(retrieveListOfSignOnRequests());
            //return View(retrieveMockedListOfSignOnRequests());
        }

        private List<SignOnMonitor.Models.SignOnRequest> retrieveMockedListOfSignOnRequests()
        {
            List<SignOnMonitor.Models.SignOnRequest> modelRequests = new List<SignOnMonitor.Models.SignOnRequest>();
            SignOnMonitor.Models.SignOnRequest mr = new SignOnMonitor.Models.SignOnRequest();
            mr.Username = "username";
            mr.DeviceIdentifier = "device id";
            mr.Success = true;
            mr.Base64Image = "imgage";
            mr.Longitude = "18.457031";
            mr.Latitude = "27.565163";

            modelRequests.Add(mr);

            return modelRequests;
        }

        private List<SignOnMonitor.Models.SignOnRequest> retrieveListOfSignOnRequests()
        {
            ISignOnRepository repository = new SignOnRepository.impl.SignOnRepository();
            List<ISignOnRequest> requests = repository.retrieveAuthenticationRequests();
            List<SignOnMonitor.Models.SignOnRequest> modelRequests = new List<SignOnMonitor.Models.SignOnRequest>();

            foreach (var r in requests)
            {
                SignOnMonitor.Models.SignOnRequest mr = new SignOnMonitor.Models.SignOnRequest();
                mr.Username = r.getUsername();
                mr.DeviceIdentifier = r.getDeviceIdentifier();
                mr.Success = r.getSuccess();
                mr.Base64Image = r.getBase64Image();
                mr.Longitude = r.getLongitude();
                mr.Latitude = r.getLatitude();
                mr.ImageAsHtml = convertToImageHtmlTag(r.getBase64Image());
                mr.Id = r.getId();

                modelRequests.Add(mr);
            }

            return modelRequests;
        }

        private string convertToImageHtmlTag(string base64)
        {
            return "<img src='data:image/Jpeg;base64," + base64 + "' />";
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ViewMap(int id)
        {
            ISignOnRepository repository = new SignOnRepository.impl.SignOnRepository();
            ISignOnRequest r = repository.retrieveAuthenticationRequestById(id);

            SignOnMonitor.Models.SignOnRequest mr = new SignOnMonitor.Models.SignOnRequest();
            mr.Username = r.getUsername();
            mr.DeviceIdentifier = r.getDeviceIdentifier();
            mr.Success = r.getSuccess();
            mr.Base64Image = r.getBase64Image();
            mr.Longitude = r.getLongitude();
            mr.Latitude = r.getLatitude();
            mr.ImageAsHtml = convertToImageHtmlTag(r.getBase64Image());
            mr.Id = r.getId();

            return View(mr);
        }

    }
}