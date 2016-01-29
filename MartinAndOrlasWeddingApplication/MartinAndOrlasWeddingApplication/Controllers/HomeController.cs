using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MartinAndOrlasWeddingApplication.Models;
using WeddingServices.Implementation;
using WeddingServices.Interface;
using System.Security.Claims;


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
                gm.ReferenceIdentifier = g.getReferenceIdentifier();
                gm.NickName = g.getNickname();
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
                    gm.ReferenceIdentifier = g.getReferenceIdentifier();
                    gm.NickName = g.getNickname();

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
                g.ReferenceIdentifier = retrievedGuest.getReferenceIdentifier();
                g.NickName = retrievedGuest.getNickname();

                if (retrievedGuest.getRelatedGuest() != null)
                {
                    MartinAndOrlasWeddingApplication.Models.Guest relatedGuest = new MartinAndOrlasWeddingApplication.Models.Guest();
                    relatedGuest.RelatedGuest = convertModels(retrievedGuest.getRelatedGuest());
                }
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

            if (retrievedGuest != null && retrievedGuest.getRelatedGuest() != null)
            {
                IGuest partner = retrievedGuest.getRelatedGuest();
                return Json(new { Name = partner.getFirstname() + " " + partner.getSurname(), Id = retrievedGuest.getIdentifier(), GuestId = partner.getIdentifier()}, JsonRequestBehavior.AllowGet);
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
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            Claim firstnameClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            Claim surnameClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
            Claim identifierClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid");

            IGuestService service = new GuestService();
            IGuest existingGuest = service.RetrieveGuestByIdentifier(Int32.Parse(identifierClaim.Value));

            return View(convertModels(existingGuest));
        }

        [HttpPost]
        public ActionResult Rsvp(MartinAndOrlasWeddingApplication.Models.Guest guest)
        {
            if (ModelState.IsValid)
            {
                IGuestService service = new GuestService();
                IGuest existingCustomer = service.RetrieveGuestByIdentifier(guest.Id);
                if(existingCustomer != null){
                    //Need to update here
                    WeddingServices.Implementation.Guest g = new WeddingServices.Implementation.Guest();
                    g.Firstname = existingCustomer.getFirstname();
                    g.Surname = existingCustomer.getSurname();
                    g.Email = existingCustomer.getEmail();
                    g.MobileNumber = existingCustomer.getMobile();
                    g.Status = guest.Status;
                    g.Id = existingCustomer.getIdentifier();
                    g.AttendingGuestIdentifier = existingCustomer.getAttendingGuestIdentifier();

                    //Need to ensure associated guest added here
                    WeddingServices.Implementation.Relationship r = new WeddingServices.Implementation.Relationship();
                    WeddingServices.Implementation.Guest cg = existingCustomer.getRelatedGuest() as WeddingServices.Implementation.Guest;
                    cg.Status = guest.RelatedGuest.Status;

                    r.RelatedGuest = cg;
                    g.Relationship = r;

                    service.UpdateGuest(g);
                } 
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

        private MartinAndOrlasWeddingApplication.Models.Guest convertModels(IGuest retrievedGuest)
        {
            MartinAndOrlasWeddingApplication.Models.Guest portalGuest = new MartinAndOrlasWeddingApplication.Models.Guest();
            portalGuest.Id = retrievedGuest.getIdentifier();
            portalGuest.Firstname = retrievedGuest.getFirstname();
            portalGuest.Surname = retrievedGuest.getSurname();
            portalGuest.Email = retrievedGuest.getEmail();
            portalGuest.MobileNumber = retrievedGuest.getMobile();
            portalGuest.Status = retrievedGuest.getStatus();

            if (retrievedGuest.getRelatedGuest() != null)
            {
                MartinAndOrlasWeddingApplication.Models.Guest relatedPortalGuest = new MartinAndOrlasWeddingApplication.Models.Guest();

                relatedPortalGuest.Id = retrievedGuest.getRelatedGuest().getIdentifier();
                relatedPortalGuest.Firstname = retrievedGuest.getRelatedGuest().getFirstname();
                relatedPortalGuest.Surname = retrievedGuest.getRelatedGuest().getSurname();
                relatedPortalGuest.Email = retrievedGuest.getRelatedGuest().getEmail();
                relatedPortalGuest.MobileNumber = retrievedGuest.getRelatedGuest().getMobile();
                relatedPortalGuest.Status = retrievedGuest.getRelatedGuest().getStatus();

                portalGuest.RelatedGuest = relatedPortalGuest;
            }

            return portalGuest;
        }

    }
}
