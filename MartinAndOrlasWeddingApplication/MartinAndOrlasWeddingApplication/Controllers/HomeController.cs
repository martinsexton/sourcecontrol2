using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MartinAndOrlasWeddingApplication.Models;
using WeddingServices.Implementation;
using WeddingServices.Interface;

namespace MartinAndOrlasWeddingApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Guests()
        {
            ViewBag.Message = "Wedding Guests";
            IGuestService service = new GuestService();
            List<IGuest> guests = service.RetrieveAllGuests();
            List<MartinAndOrlasWeddingApplication.Models.Guest> modelGuests = new List<MartinAndOrlasWeddingApplication.Models.Guest>();
            foreach(var g in guests){
                MartinAndOrlasWeddingApplication.Models.Guest gm = new MartinAndOrlasWeddingApplication.Models.Guest();
                gm.Email = g.getEmail();
                gm.Firstname = g.getFirstname();
                gm.Surname = g.getSurname();
                gm.Status = g.getStatus();

                modelGuests.Add(gm);
            }

            return View(modelGuests);
        }

        public ActionResult Guest(int id)
        {
            MartinAndOrlasWeddingApplication.Models.Guest g = GenerateTestGuest();
            ViewBag.Message = g.Firstname + " " + g.Surname;
            return View(GenerateTestGuest());
        }

        public ActionResult AddGuest()
        {
            return View();
        }

        public ActionResult Rsvp()
        {
            return View();
        }

        public ActionResult Event()
        {
            return View();
        }

        private List<MartinAndOrlasWeddingApplication.Models.Guest> GenerateListOfGuests()
        {
            List<MartinAndOrlasWeddingApplication.Models.Guest> guests = new List<MartinAndOrlasWeddingApplication.Models.Guest>();
            guests.Add(GenerateTestGuest());
            guests.Add(GenerateTestGuest());
            guests.Add(GenerateTestGuest());
            guests.Add(GenerateTestGuest());
            return guests;
        }

        private MartinAndOrlasWeddingApplication.Models.Guest GenerateTestGuest()
        {
            MartinAndOrlasWeddingApplication.Models.Guest testGuest = new MartinAndOrlasWeddingApplication.Models.Guest();
            testGuest.Id = 3;
            testGuest.Firstname = "Martin";
            testGuest.Surname = "Sexton";
            testGuest.MobileNumber = "087 7934934";
            testGuest.Email = "themairt@hotmail.com";
            testGuest.Status = "Accepted";
            testGuest.FoodChoice = "Beef";

            return testGuest;
        }

    }
}