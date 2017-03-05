using BigDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingServices.Interface;
using System.Security.Claims;
using EmailService.Interface;

namespace BigDay.Controllers
{
    public class HomeController : Controller
    {
        IGuestService service;
        IEmailService emailService;

        //Inject relevant services into controller via constructor
        public HomeController(IGuestService s, IEmailService es)
        {
            service = s;
            emailService = es;
        }

        public ActionResult Index()
        {
            return View();
        }

<<<<<<< HEAD
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

=======

        public ActionResult ChurchDetails()
        {
            return View();
        }

        public ActionResult ReceptionDetails()
        {
>>>>>>> c3571c688adaa767c8b3384d6b31352b4edcacfe
            return View();
        }

        [HttpGet]
        public ActionResult Rsvp()
        {
            return View(new WeddingGuest());
        }

<<<<<<< HEAD
=======
        [HttpGet]
        public ActionResult Map()
        {
            return View();
        }

>>>>>>> c3571c688adaa767c8b3384d6b31352b4edcacfe
        [HttpPost]
        public ActionResult Rsvp(Models.WeddingGuest guest)
        {
            if (ModelState.IsValid)
            {
                IGuest g = convertToPersistantModel(guest);
                service.AddGuest(g);
                //service.UpdateGuest(g);

                return View("RsvpResult", guest);
            }
            else
            {
                return View(guest);
            }
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
                List<IGuest> guests = service.RetrieveGuestsByStatus(filterbutton);
                List<Models.WeddingGuest> modelGuests = new List<Models.WeddingGuest>();
                foreach (var g in guests)
                {
                    Models.WeddingGuest gm = new Models.WeddingGuest();
                    gm.Id = g.getIdentifier();
                    gm.Firstname = g.getFirstname();
                    gm.Surname = g.getSurname();
                    gm.Status = g.getStatus();

                    modelGuests.Add(gm);
                }

                return View(modelGuests);
            }
        }

        [HttpGet]
        public ActionResult Guests()
        {
            ViewBag.Message = "Wedding Guests";
            List<IGuest> guests = service.RetrieveAllGuests();
            List<Models.WeddingGuest> modelGuests = new List<Models.WeddingGuest>();
            foreach (var g in guests)
            {
                Models.WeddingGuest gm = new Models.WeddingGuest();
                gm.Id = g.getIdentifier();
                gm.Firstname = g.getFirstname();
                gm.Surname = g.getSurname();
                gm.Status = g.getStatus();
                modelGuests.Add(gm);
            }

            return View(modelGuests);
        }

<<<<<<< HEAD
=======

>>>>>>> c3571c688adaa767c8b3384d6b31352b4edcacfe
        private Models.WeddingGuest convertToViewModel(IGuest g)
        {
            Models.WeddingGuest guest = new WeddingGuest();
            guest.Firstname = g.getFirstname();
            guest.Surname = g.getSurname();
            guest.DietComment = g.getDietComment();
            guest.Status = g.getStatus();
            guest.Id = g.getIdentifier();
            guest.GuestName = g.getGuestName();

            return guest;
        }

        private IGuest convertToPersistantModel(Models.WeddingGuest guest)
        {
            WeddingServices.Implementation.Guest g = new WeddingServices.Implementation.Guest();
            g.Firstname = guest.Firstname;
            g.Surname = guest.Surname;
            g.Status = guest.Status;
            g.Id = guest.Id;
            g.DietComment = guest.DietComment;
            
            if (guest.IncludeGuest)
            {
                g.GuestName = guest.GuestName;
            }

            return g;
        }

        public ActionResult Guest(int id)
        {
            IGuest retrievedGuest = service.RetrieveGuestByIdentifier(id);
            Models.WeddingGuest g = convertToViewModel(retrievedGuest);

            ViewBag.Message = g.Firstname + " " + g.Surname;
            return View(g);
        }
    }
}