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

        [HttpGet]
        public ActionResult Guests()
        {
            ViewBag.Message = "Wedding Guests";
            IGuestService service = new GuestService();
            List<IGuest> guests = service.RetrieveAllGuests();
            List<MartinAndOrlasWeddingApplication.Models.Guest> modelGuests = new List<MartinAndOrlasWeddingApplication.Models.Guest>();
            foreach(var g in guests){
                MartinAndOrlasWeddingApplication.Models.Guest gm = new MartinAndOrlasWeddingApplication.Models.Guest();
                gm.Id = g.getIdentifier();
                gm.Email = g.getEmail();
                gm.Firstname = g.getFirstname();
                gm.Surname = g.getSurname();
                gm.Status = g.getStatus();
                gm.MobileNumber = g.getMobile();
                gm.AttendingGuest = g.getAttendingGuestName();
                gm.ReferenceIdentifier = g.getReferenceIdentifier();
                modelGuests.Add(gm);
            }

            return View(modelGuests);
        }

        [HttpPost]
        public ActionResult Guests(string filterbutton)
        {
            if (filterbutton.Equals("All"))
            {
                return Guests();
            }
            else
            {
                ViewBag.Message = "Wedding Guests";
                IGuestService service = new GuestService();
                List<IGuest> guests = service.RetrieveGuestsByStatus(filterbutton);
                List<MartinAndOrlasWeddingApplication.Models.Guest> modelGuests = new List<MartinAndOrlasWeddingApplication.Models.Guest>();
                foreach (var g in guests)
                {
                    MartinAndOrlasWeddingApplication.Models.Guest gm = new MartinAndOrlasWeddingApplication.Models.Guest();
                    gm.Id = g.getIdentifier();
                    gm.Email = g.getEmail();
                    gm.Firstname = g.getFirstname();
                    gm.Surname = g.getSurname();
                    gm.Status = g.getStatus();
                    gm.MobileNumber = g.getMobile();
                    gm.AttendingGuest = g.getAttendingGuestName();
                    gm.ReferenceIdentifier = g.getReferenceIdentifier();
                    modelGuests.Add(gm);
                }

                return View(modelGuests);
            }
        }

        public ActionResult Guest(int id)
        {
            IGuestService service = new GuestService();
            IGuest retrievedGuest = service.RetrieveGuestByIdentifier(id);

            MartinAndOrlasWeddingApplication.Models.Guest g = new MartinAndOrlasWeddingApplication.Models.Guest();
            if (retrievedGuest != null)
            {
                g.Firstname = retrievedGuest.getFirstname();
                g.Surname = retrievedGuest.getSurname();
                g.Email = retrievedGuest.getEmail();
                g.MobileNumber = retrievedGuest.getMobile();
                g.Status = retrievedGuest.getStatus();
                g.AttendingGuest = retrievedGuest.getAttendingGuestName();
                g.ReferenceIdentifier = retrievedGuest.getReferenceIdentifier();
            }
            ViewBag.Message = g.Firstname + " " + g.Surname;
            return View(g);
        }

        [HttpGet]
        public ActionResult AddGuest()
        {
            return View(new MartinAndOrlasWeddingApplication.Models.Guest());
        }

        public ActionResult RetrieveRelatedGuest(string firstname, string surname)
        {
            IGuestService service = new GuestService();
            IGuest retrievedGuest = service.RetrieveGuestByName(firstname, surname);

            if (retrievedGuest != null)
            {
                IGuest partner = service.RetrievePartner(retrievedGuest.getReferenceIdentifier());
                return Json(new { Name = partner.getFirstname() + " " + partner.getSurname() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Name = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddGuest(MartinAndOrlasWeddingApplication.Models.Guest guest)
        {
            if (ModelState.IsValid)
            {
                //Save Guest Here
                IGuestService service = new GuestService();
                WeddingServices.Implementation.Guest destinationGuest = new WeddingServices.Implementation.Guest();
                destinationGuest.Firstname = guest.Firstname;
                destinationGuest.Surname = guest.Surname;
                destinationGuest.Email = guest.Email;
                destinationGuest.MobileNumber = guest.MobileNumber;
                destinationGuest.Status = guest.Status;
                if (guest.AttendingGuest == null)
                {
                    destinationGuest.AttendingGuestName = "";
                }
                else
                {
                    destinationGuest.AttendingGuestName = guest.AttendingGuest;
                }
                

                service.AddGuest(destinationGuest);
                return View("GuestAddedConfirmation", guest);
            }
            else
            {
                return View(guest);
            }
        }

        [HttpGet]
        public ActionResult Rsvp()
        {
            return View(new MartinAndOrlasWeddingApplication.Models.Guest());
        }

        [HttpPost]
        public ActionResult Rsvp(MartinAndOrlasWeddingApplication.Models.Guest guest)
        {
            if (ModelState.IsValid)
            {
                //Save Guest Here
                IGuestService service = new GuestService();
                WeddingServices.Implementation.Guest destinationGuest = new WeddingServices.Implementation.Guest();
                destinationGuest.Firstname = guest.Firstname;
                destinationGuest.Surname = guest.Surname;
                destinationGuest.Email = guest.Email;
                destinationGuest.MobileNumber = guest.MobileNumber;
                destinationGuest.Status = guest.Status;
                if (guest.AttendingGuest == null)
                {
                    destinationGuest.AttendingGuestName = "";
                }
                else
                {
                    destinationGuest.AttendingGuestName = guest.AttendingGuest;
                }

                service.AddGuest(destinationGuest);

                return View("GuestAddedConfirmation", guest);
            }
            else
            {
                return View(guest);
            }
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